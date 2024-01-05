using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sdLitica.Entities.TimeSeries;
using sdLitica.PlatformCore;
using sdLitica.TimeSeries.Services;
using sdLitica.Utils.Settings;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Entities.Common.Pages;
using sdLitica.WebAPI.Models.TimeSeries;
using sdLitica.TimeSeries.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;
using TimeSeriesJTSEntity = sdLitica.TimeSeries.Services.TimeSeriesJTSEntity;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is used to work with user timeseries
    /// </summary>
    [Route("api/v1/timeseries")]
    [Authorize]
    public class TimeSeriesController : BaseApiController
    {
        private readonly ITimeSeriesService _timeSeriesService;
        private readonly ITimeSeriesMetadataService _timeSeriesMetadataService;
        private readonly IBucketMetadataService _bucketMetadataService;
        private readonly UserService _userService;

        public TimeSeriesController(ITimeSeriesService timeSeriesService, 
            ITimeSeriesMetadataService timeSeriesMetadataService, 
            IBucketMetadataService bucketMetadataService, 
            UserService userService)
        {
            _timeSeriesService = timeSeriesService;
            _timeSeriesMetadataService = timeSeriesMetadataService;
            _bucketMetadataService = bucketMetadataService;
            _userService = userService;
        }

        /// <summary>
        /// This REST API handler creates a new timeseries metadata object for current user
        /// </summary>
        /// <param name="timeSeriesModel"></param>
        /// <returns>
        ///    200 - Timeseries was successfully created. Response payload will contain object with assigned id
        ///    400 - There were issues with passed data, i.e. required fields missing or length constraints violated
        /// </returns>
        [HttpPost]
        public IActionResult AddTimeSeries([FromBody] TimeSeriesMetadataModel timeSeriesModel)
        {
            Task<TimeSeriesMetadata> t = _timeSeriesMetadataService.AddTimeseriesMetadata(timeSeriesModel.Name, timeSeriesModel.Description, timeSeriesModel.BucketId, timeSeriesModel.Type);
            _timeSeriesService.AddRandomTimeSeries(t.Result.InfluxId.ToString(), timeSeriesModel.BucketId);
            return Ok(new TimeSeriesMetadataModel(t.Result));
        }

        /// <summary>
        /// This REST API handler uploads a data from csv-file to the timeseries given by timeSeriesMetadataId
        /// </summary>
        /// <param name="timeSeriesMetadataId"></param>
        /// <param name="formFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{timeSeriesMetadataId}/data/file")]
        public async Task<IActionResult> UploadCsvData([FromRoute] string timeSeriesMetadataId, [FromForm] IFormFile formFile)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("this user does not have timeseries given by this id");
            }
            
            string measurementId = timeSeriesMetadata.InfluxId.ToString();
            List<string> fileContent = await ReadAsStringAsync(formFile);
            
            var result = await _timeSeriesService.UploadDataFromCsv(measurementId, bucketMetadata.InfluxId, fileContent);
            
            var t = _timeSeriesMetadataService.UpdateTimeSeriesMetadata(timeSeriesMetadata.Id.ToString(),
                timeSeriesMetadata.Name, timeSeriesMetadata.Description, result.rowCount, result.columnsCount,
                result.columns, result.tags);
            return Ok(new TimeSeriesMetadataModel(t.Result));
        }

        //temporarily here. consider move to sdLitica.Helpers
        public static async Task<List<string>> ReadAsStringAsync(IFormFile file)
        {
            List<string> result = new List<string>();
            using (StreamReader reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    string line = await reader.ReadLineAsync();
                    result.Add(line);
                }
            }
            return result;
        }

        /// <summary>
        /// This REST API handler uploads a data from JTS (json timeseries) to the timeseries given by timeSeriesMetadataId
        /// </summary>
        /// <param name="timeSeriesMetadataId"></param>
        /// <param name="timeSeriesJts"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{timeSeriesMetadataId}/data/jts")]
        public async Task<IActionResult> UploadJsonTsData([FromRoute] string timeSeriesMetadataId, [FromBody] TimeSeriesJTSEntity timeSeriesJts)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("this user does not have time series given by this id");
            }
        
            var result = await _timeSeriesService.UploadDataFromJTs(timeSeriesMetadata.InfluxId.ToString(), bucketMetadata.InfluxId, timeSeriesJts);
            
            var t = _timeSeriesMetadataService.UpdateTimeSeriesMetadata(timeSeriesMetadata.Id.ToString(),
                timeSeriesMetadata.Name, timeSeriesMetadata.Description, result.rowCount, result.columnsCount,
                result.columns, result.tags);
        
            return Ok(new TimeSeriesMetadataModel(t.Result));
        }

        [Route("{timeSeriesMetadataId}/data/ws")]
        [HttpPost]
        public async Task getFromWebSocket([FromRoute] string timeSeriesMetadataId)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }

            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
            }
            
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                var buffer = new byte[1024 * 4];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                

                var t = JsonConvert.DeserializeObject<TimeSeriesWebSocketEntity>(message);

                var resultUpload = await _timeSeriesService.UploadDataFromWebSocket(timeSeriesMetadata.Id.ToString(), bucketMetadata.InfluxId, t);

                _timeSeriesMetadataService.UpdateTimeSeriesMetadata(timeSeriesMetadata.Id.ToString(),
                        timeSeriesMetadata.Name, timeSeriesMetadata.Description, resultUpload.rowCount, 
                        resultUpload.columnsCount, resultUpload.columns, resultUpload.tags);

                try
                {
                    while (!result.CloseStatus.HasValue)
                    {
                        buffer = new byte[1024 * 4];
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                        await Task.Delay(1000);

                        t = JsonConvert.DeserializeObject<TimeSeriesWebSocketEntity>(message);

                        resultUpload =
                            await _timeSeriesService.UploadDataFromWebSocket(timeSeriesMetadata.Id.ToString(),
                                bucketMetadata.InfluxId, t);

                        _timeSeriesMetadataService.UpdateTimeSeriesMetadata(timeSeriesMetadata.Id.ToString(),
                            timeSeriesMetadata.Name, timeSeriesMetadata.Description, resultUpload.rowCount,
                            resultUpload.columnsCount, resultUpload.columns, resultUpload.tags);
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex);
                }

                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
                Console.WriteLine("WebSocket connection closed");
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }

        /// <summary>
        /// This REST API handler returns all timeseries metadata objects owned by bucket
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("bucket/{bucketMetadataId}")]
        public IActionResult GetTimeSeriesMetadataByBucket([FromRoute] string bucketMetadataId)
        {
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(bucketMetadataId);
            if (!(bucketMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("this user does not have timeseries given by this id");
            }
            List<TimeSeriesMetadata> t = _timeSeriesMetadataService.GetByBucketId(bucketMetadataId);
            List<TimeSeriesMetadataModel> list = new List<TimeSeriesMetadataModel>();
            t.ForEach(e => list.Add(new TimeSeriesMetadataModel(e)));
            return Ok(list);
        }

        /// <summary>
        /// This REST API handler replace metadata for timeseries identified by ts-id
        /// </summary>
        /// <returns>
        ///    200 - Timeseries was successfully updated. Response payload will contain updated timeseries entry
        ///    400 - There were issues with passed data, i.e. required fields missing or length constraints violated
        ///    404 - If time series doesn't exists or it is not accessible by current user
        /// </returns>
        [HttpPost]
        [Route("{timeSeriesMetadataId}")]
        public IActionResult UpdateTimeSeriesMetadata([FromRoute] string timeSeriesMetadataId, [FromBody] TimeSeriesMetadataModel model)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("this user does not have timeseries given by this id");
            }
            
            _timeSeriesMetadataService.UpdateTimeSeriesMetadata(timeSeriesMetadataId, model.Name, model.Description).Wait();
            return Ok("ok");
        }

        /// <summary>
        /// This REST API handler returns timeseries metadata given by timeSeriesMetadataId
        /// </summary>
        /// <param name="timeSeriesMetadataId"></param>
        /// <returns>
        ///    200 - Timeseries metadata
        ///    404 - If time series doesn't exists or it is not accessible by current user
        /// </returns>
        [HttpGet]
        [Route("{timeSeriesMetadataId}")]
        public IActionResult GetTimeSeriesMetadataById([FromRoute] string timeSeriesMetadataId)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("this user does not have timeseries given by this id");
            }
            
            return Ok(new TimeSeriesMetadataModel(timeSeriesMetadata));
        }
        
        [HttpGet]
        [Route("{timeSeriesMetadataId}/link")]
        public IActionResult getTimeSeriesLink([FromRoute] string timeSeriesMetadataId, [FromQuery] string typeLink)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("this user does not have timeseries given by this id");
            }
            
            var uri = typeLink.Equals("stream") ? $"ws://{Request.Host}/api/v1/timeseries/{timeSeriesMetadataId}/data/ws" : $"http://{Request.Host}/api/v1/timeseries/{timeSeriesMetadataId}/data/jts";
            var apiKey = _userService.CreateUserApiKey(UserId, $"For time series: {timeSeriesMetadata.Id}");
            
            return Ok(new { Uri = uri, ApiKey = apiKey.APIKey });
        }

        /// <summary>
        /// This REST API handler returns all timeseries data by timeseries id
        /// </summary>
        /// <param name="timeseriesId">id of timeseries metadata</param>
        /// <param name="pageSize"></param>
        /// <param name="offset"></param>
        /// <returns>Timeseries data, instead - 404</returns>
        [HttpGet]
        [Route("{timeseriesId}/data/all")]
        public IActionResult GetAllTimeSeriesDataById(string timeseriesId)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeseriesId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata =
                _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            List<FluxTable> measurementsResult = _timeSeriesService.ReadMeasurementById(timeSeriesMetadata.InfluxId.ToString(), bucketMetadata.InfluxId).Result;
            return MakePageFromMeasurements(measurementsResult, timeSeriesMetadata.RowsCount, 0);
        }

        /// <summary>
        /// This REST API handler returns timeseries data by timeseries id
        /// </summary>
        /// <param name="timeseriesId">id of timeseries metadata</param>
        /// <param name="from">starting point of date-time interval. format: https://docs.influxdata.com/influxdb/v1.8/query_language/explore-data#time-syntax</param>
        /// <param name="to">end point of date-time interval. format: https://docs.influxdata.com/influxdb/v1.8/query_language/explore-data#time-syntax</param>
        /// <param name="step">step of date-time interval. format: https://docs.influxdata.com/influxdb/v1.8/query_language/spec/#durations</param>
        /// <param name="pageSize"></param>
        /// <param name="offset"></param>
        /// <returns>Timeseries data, instead - 404</returns>
        [HttpGet]
        [Route("{timeseriesId}/data")]
        public IActionResult GetTimeSeriesDataById(string timeseriesId, string from = "", string to = "", string step = "", string agrFn = "", int pageSize = 20, int offset = 0)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeseriesId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            List<FluxTable> measurementsResult = _timeSeriesService.ReadMeasurementById(timeSeriesMetadata.InfluxId.ToString(),bucketMetadata.InfluxId, from, to, step, agrFn).Result;
            return MakePageFromMeasurements(measurementsResult, pageSize, offset);
        }
        
        [HttpGet]
        [Route("{timeseriesId}/download")]
        public IActionResult DownloadCsvTimeSeries(string timeseriesId)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeseriesId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            var fileBytes = Encoding.UTF8.GetBytes(_timeSeriesService.CreateFileCSV(timeSeriesMetadata.InfluxId.ToString(),bucketMetadata.InfluxId).Result);

            return File(fileBytes, "text/csv", timeSeriesMetadata.Name + ".csv");
        }

        /// <summary>
        /// This REST API handler delete metadata for timeseries identified by ts-id with all its assigned/uploaded data.
        /// </summary>
        /// <returns>
        ///    204 - Timeseries was successfully deleted. No response expected from server
        ///    404 - If time series doesn't exists or it is not accessible by current user
        /// </returns>
        [HttpDelete]
        [Route("{timeSeriesMetadataId}")]
        public IActionResult DeleteTimeSeriesById([FromRoute] string timeSeriesMetadataId)
        {
            TimeSeriesMetadata timeSeriesMetadata = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId);
            if (timeSeriesMetadata == null) { 
                return NotFound("this user does not have time series given by this id");
            }
            
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(timeSeriesMetadata.BucketId.ToString());
            if (!(timeSeriesMetadata != null && bucketMetadata.UserId.ToString().Equals(UserId)))
            {
                return NotFound("user does not have this timeseries");
            }

            string timeSeriesId = timeSeriesMetadata.InfluxId.ToString();
            _timeSeriesService.DeleteMeasurementById(timeSeriesId, bucketMetadata.InfluxId);

            _timeSeriesMetadataService.DeleteTimeSeriesMetadata(timeSeriesMetadataId).Wait();
            return NoContent();
        }

        private IActionResult MakePageFromMeasurements(List<FluxTable> measurementsResult, int pageSize, int offset)
        {
            if (measurementsResult.Count == 0) return NotFound();

            List<TimeSeriesDataJsonEntity> timeseriesDataJsonEntities = new List<TimeSeriesDataJsonEntity>();
            foreach (FluxTable table in measurementsResult)
            {
                List<FluxRecord> records = table.Records;
                foreach (FluxRecord record in records)
                {
                    var tags = record.Values
                        .Where(x =>
                            !x.Key.StartsWith('_') &&
                            x.Key != "result" &&
                            x.Key != "table")
                        .ToDictionary(x => x.Key, x => x.Value.ToString());

                    var timestamp = record.GetTimeInDateTime().ToString();

                    var timeseriesDataJsonEntity = timeseriesDataJsonEntities.Find(td =>
                        // td.Timestamp == timestamp);
                        td.Timestamp == timestamp &&
                        td.Tags.SequenceEqual<KeyValuePair<string, string>>(tags));
                    
                    // Console.WriteLine(timestamp + " = " + record.GetField() + " - " + record.GetValue());
                    

                    if (timeseriesDataJsonEntity == null)
                    {
                        var fields = new Dictionary<string, object>();
                        fields.Add(record.GetField(), record.GetValue());

                        timeseriesDataJsonEntities.Add(new TimeSeriesDataJsonEntity
                        {
                            Timestamp = timestamp,
                            Fields = fields,
                            Tags = tags
                        });
                    }
                    else
                    {
                        try
                        {
                            timeseriesDataJsonEntity.Fields.Add(record.GetField(), record.GetValue());
                        }
                        catch (Exception e)
                        {
                        }
                    }
                }
            }

            List<TimeSeriesDataJsonEntity> page = timeseriesDataJsonEntities.Skip(offset).Take(pageSize).ToList();

            ApiEntityListPage<TimeSeriesDataJsonEntity> listOfResults =
                new ApiEntityListPage<TimeSeriesDataJsonEntity>(page,
                    HttpContext.Request.Path.ToString(), new PaginationProperties()
                    {
                        PageSize = pageSize,
                        Offset = offset,
                        Count = page.Count,
                        HasMore = timeseriesDataJsonEntities.Count > page.Count
                    });

            return Ok(listOfResults);
        }
    }
}