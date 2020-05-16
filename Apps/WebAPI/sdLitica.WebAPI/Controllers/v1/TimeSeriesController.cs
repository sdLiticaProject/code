using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sdLitica.TimeSeries.Services;
using sdLitica.WebAPI.Entities.Common;
using sdLitica.WebAPI.Entities.Common.Pages;
using sdLitica.WebAPI.Models.Management;
using sdLitica.WebAPI.Models.TimeSeries;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

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

        public TimeSeriesController(ITimeSeriesService timeSeriesService)
        {
            _timeSeriesService = timeSeriesService;
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
            Task<string> t = _timeSeriesService.AddRandomTimeSeries();
            return Ok(t.Result);
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
        public IActionResult UpdateTimeSeriesMetadata(string timeseriesId)
        {
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
            InfluxResult<DynamicInfluxRow> measurementsResult = _timeSeriesService.ReadMeasurementById(timeseriesId).Result;
            {
                List<InfluxSeries<DynamicInfluxRow>> series = measurementsResult.Series;
                if (series.Count != 0)
                {
                    List<TimeSeriesJsonEntity> timeseriesJsonEntities = new List<TimeSeriesJsonEntity>();
                    foreach (InfluxSeries<DynamicInfluxRow> t in series)
                    {
                        List<DynamicInfluxRow> rows = t.Rows;
                        foreach (DynamicInfluxRow t1 in rows)
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

                    List<TimeSeriesJsonEntity> page = timeseriesJsonEntities.Skip(offset).Take(pageSize).ToList();

                    ApiEntityListPage<TimeSeriesJsonEntity> listOfResults =
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
            InfluxResult<DynamicInfluxRow> measurementsResult = _timeSeriesService.ReadMeasurementById(timeseriesId).Result;
            {
                List<InfluxSeries<DynamicInfluxRow>> series = measurementsResult.Series;
                if (series.Count != 0)
                {
                    List<TimeSeriesDataJsonEntity> timeseriesDataJsonEntities = new List<TimeSeriesDataJsonEntity>();
                    foreach (InfluxSeries<DynamicInfluxRow> t in series)
                    {
                        List<DynamicInfluxRow> rows = t.Rows;
                        foreach (DynamicInfluxRow t1 in rows)
                        {
                            timeseriesDataJsonEntities.Add(new TimeSeriesDataJsonEntity()
                            {
                                Timestamp = t1.Timestamp.ToString(),
                                Tags = t1.Tags,
                                Fields = t1.Fields
                            });
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
            List<MeasurementRow> measurementsResult = _timeSeriesService.ReadAllMeasurements().Result;
            {
                MeasurementRow[] ms = measurementsResult.ToArray();
                List<string> mt = ms.Select(t => t.Name).ToList();
                List<MeasurementJsonEntity> measurementJsonEntities = mt.Select(t => new MeasurementJsonEntity() {Guid = t}).ToList();

                List<MeasurementJsonEntity> page = measurementJsonEntities.Skip(offset).Take(pageSize).ToList();

                ApiEntityListPage<MeasurementJsonEntity> listOfResults =
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
        public IActionResult DeleteTimeSeriesById(string timeseriesId)
        {
            InfluxResult result = _timeSeriesService.DeleteMeasurementById(timeseriesId).Result;
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }
    }
}