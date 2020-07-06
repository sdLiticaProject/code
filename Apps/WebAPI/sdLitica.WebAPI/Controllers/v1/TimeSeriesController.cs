using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Entities.TimeSeries;
using sdLitica.TimeSeries.Services;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Entities.Common.Pages;
using sdLitica.WebAPI.Models.TimeSeries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is used to work with user timeseries
    /// </summary>
    [Route("api/v1/timeseries")]
    //TODO: Change auth settings here
    [AllowAnonymous]
    public class TimeSeriesController : BaseApiController
    {
        private readonly ITimeSeriesService _timeSeriesService;
        private readonly ITimeSeriesMetadataService _timeSeriesMetadataService;

        public TimeSeriesController(ITimeSeriesService timeSeriesService, ITimeSeriesMetadataService timeSeriesMetadataService)
        {
            _timeSeriesService = timeSeriesService;
            _timeSeriesMetadataService = timeSeriesMetadataService;
        }

        /// <summary>
        /// This REST API handler creates a new time-series object for current user
        /// </summary>
        /// <returns>
        ///    200 - Time-series was successfully created. Response payload will contain object with assigned id
        ///    400 - There were issues with passed data, i.e. required fields missing or length constraints violated
        /// </returns>
        [HttpPost]
        public IActionResult AddTimeSeries()
        {
            var t = _timeSeriesService.AddRandomTimeSeries();
            return Ok(t.Result);
        }

        [HttpPost]
        [Route("temp")]
        public IActionResult AddTimeSeries([FromBody] TimeSeriesMetadataModel timeSeriesModel) {
            Task<TimeSeriesMetadata> t = _timeSeriesMetadataService.AddTimeseriesMetadata(timeSeriesModel.Name, UserId);
            _timeSeriesService.AddRandomTimeSeries(t.Result.InfluxId.ToString());
            return Ok(new TimeSeriesMetadataModel(t.Result));
        }

        [HttpPost]
        [Route("{measurementId}/data")]
        public IActionResult UploadCsvData([FromRoute] string measurementId, [FromForm] IFormFile formFile)
        {
            // todo: check if authorized user owns this time-series, update rows and columns metadata after extraction
            List<string> fileContent = ReadAsStringAsync(formFile).Result;
            _timeSeriesService.UploadDataFromCsv(measurementId, fileContent);
            return Ok();
        }

        //temporary here. consider move to sdLitica.Helpers
        public static async Task<List<string>> ReadAsStringAsync(IFormFile file)
        {
            List<string> result = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    string line = await reader.ReadLineAsync();
                    result.Add(line);
                }
            }
            return result;
        }

        [HttpGet]
        [Route("temp")]
        public IActionResult GetTimeSeriesMetadataByUser()
        {
            List<TimeSeriesMetadata> t = _timeSeriesMetadataService.GetByUserId(UserId);
            List<TimeSeriesMetadataModel> list = new List<TimeSeriesMetadataModel>();
            t.ForEach(e => list.Add(new TimeSeriesMetadataModel(e)));
            return Ok(list);
        }

        /// <summary>
        /// This REST API handler replace metadata for timeseries identified by ts-id
        /// </summary>
        /// <returns>
        ///    200 - Time-series was successfully updated. Response payload will contain updated time-series entry
        ///    400 - There were issues with passed data, i.e. required fields missing or length constraints violated
        ///    404 - If time series doesn't exists or it is not accessible by current user
        /// </returns>
        [HttpPost]
        [Route("{timeseriesId}")]
        //TODO: add content
        public IActionResult UpdateTimeSeriesMetadata([FromBody] TimeSeriesMetadataModel model)
        {
            _timeSeriesMetadataService.UpdateTimeSeriesMetadata(model.Id, model.Name, model.Description).Wait();
            return Ok("ok");
        }

        /// <summary>
        /// This REST API handler returns timeseries metadata by id
        /// </summary>
        /// <returns>Timeseries metadata, instead - 404</returns>
        [HttpGet]
        [Route("{timeseriesId}")]
        public IActionResult GetTimeSeriesMetadataById(string timeseriesId, int pageSize = 20, int offset = 0)
        {
            var measurementsResult = _timeSeriesService.ReadMeasurementById(timeseriesId).Result;
            {
                var series = measurementsResult.Series;
                if (series.Count != 0)
                {
                    var timeseriesJsonEntities = new List<TimeSeriesJsonEntity>();
                    foreach (var t in series)
                    {
                        var rows = t.Rows;
                        foreach (var t1 in rows)
                        {
                            timeseriesJsonEntities.Add(new TimeSeriesJsonEntity()
                            {
                                Id = Guid.NewGuid().ToString(),
                                Name = "",
                                Description = "",
                                Tags = t1.Tags
                            });
                        }
                    }

                    var page = timeseriesJsonEntities.Skip(offset).Take(pageSize).ToList();

                    var listOfResults =
                        new ApiEntityListPage<TimeSeriesJsonEntity>(page,
                            HttpContext.Request.Path.ToString(), new PaginationProperties()
                            {
                                PageSize = pageSize,
                                Offset = offset,
                                Count = page.Count,
                                HasMore = timeseriesJsonEntities.Count > page.Count
                            });

                    return Ok(listOfResults);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        /// <summary>
        /// This REST API handler returns timeseries data by id
        /// </summary>
        /// <returns>Timeseries data, instead - 404</returns>
        [HttpGet]
        [Route("{timeseriesId}/data")]
        public IActionResult GetTimeSeriesDataById(string timeseriesId, int pageSize = 20, int offset = 0)
        {
            var measurementsResult = _timeSeriesService.ReadMeasurementById(timeseriesId).Result;
            {
                var series = measurementsResult.Series;
                if (series.Count != 0)
                {
                    List<TimeSeriesDataJsonEntity> timeseriesDataJsonEntities = new List<TimeSeriesDataJsonEntity>();
                    foreach (var t in series)
                    {
                        var rows = t.Rows;
                        foreach (var t1 in rows)
                        {
                            timeseriesDataJsonEntities.Add(new TimeSeriesDataJsonEntity()
                            {
                                Timestamp = t1.Timestamp.ToString(),
                                Tags = t1.Tags,
                                Fields = t1.Fields
                            });
                        }
                    }

                    var page = timeseriesDataJsonEntities.Skip(offset).Take(pageSize).ToList();

                    var listOfResults =
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
                else
                {
                    return NotFound();
                }
            }
        }

        /// <summary>
        /// This REST API handler returns list of all timeseries
        /// </summary>
        /// <returns>List of timeseries</returns>
        [HttpGet]
        public IActionResult GetAllTimeSeries(int pageSize = 20, int offset = 0)
        {
            var measurementsResult = _timeSeriesService.ReadAllMeasurements().Result;
            {
                var ms = measurementsResult.ToArray();
                var mt = ms.Select(t => t.Name).ToList();
                var measurementJsonEntities = mt.Select(t => new MeasurementJsonEntity() {Guid = t}).ToList();

                var page = measurementJsonEntities.Skip(offset).Take(pageSize).ToList();

                var listOfResults =
                    new ApiEntityListPage<MeasurementJsonEntity>(page,
                        HttpContext.Request.Path.ToString(), new PaginationProperties()
                        {
                            PageSize = pageSize,
                            Offset = offset,
                            Count = page.Count,
                            HasMore = measurementJsonEntities.Count > page.Count
                        });

                return Ok(listOfResults);
            }
        }

        /// <summary>
        /// This REST API handler delete metadata for timeseries identified by ts-id with all its assigned/uploaded data.
        /// </summary>
        /// <returns>
        ///    204 - Time-series was successfully deleted. No response expected from server
        ///    404 - If time series doesn't exists or it is not accessible by current user
        /// </returns>
        [HttpDelete]
        [Route("{timeseriesId}")]
        public IActionResult DeleteTimeSeriesById(string timeSeriesMetadataId)
        {
            string timeSeriesId = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesMetadataId).InfluxId.ToString();
            var result = _timeSeriesService.DeleteMeasurementById(timeSeriesId).Result;
            if (result.Succeeded)
            {
                _timeSeriesMetadataService.DeleteTimeSeriesMetadata(timeSeriesMetadataId).Wait();
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}