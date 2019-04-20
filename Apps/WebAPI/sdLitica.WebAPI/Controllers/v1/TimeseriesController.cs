using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sdLitica.WebAPI.Services;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is used to work with user timeseries
    /// </summary>
    [Route("api/v1/timeseries")]
    //TODO: Change auth settings here
    [AllowAnonymous]
    public class TimeseriesController : BaseApiController
    {
        private readonly IInfluxDB _influxDb;

        public TimeseriesController(IInfluxDB influxDb)
        {
            _influxDb = influxDb;
        }

        /// <summary>
        /// This REST API handler returns timeseries id
        /// </summary>
        /// <returns>Timeseries id</returns>
        [HttpPost]
        public IActionResult AddTimeseries()
        {
            var t = _influxDb.AddRandomTimeseries();
            return Ok(t.Result);
        }

        /// <summary>
        /// This REST API handler returns timeseries by id
        /// </summary>
        /// <returns>Timeseries, instead - 404</returns>
        [HttpGet]
        [Route("{timeseriesId}")]
        public IActionResult GetTimeseriesById(string timeseriesId)
        {
            var measurementsResult = _influxDb.ReadMeasurementById(timeseriesId).Result;
            {
                var series = measurementsResult.Series;
                if (series.Count != 0)
                {
                    var json = JsonConvert.SerializeObject(
                        series[0].Rows.Select(ll => new {ll.Timestamp, ll.Fields, ll.Tags})
                        , Formatting.Indented);
                    return Ok(json);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        /// <summary>
        /// This REST API handler drops timeseries by id
        /// </summary>
        /// <returns>204 if succeed, instead - 404</returns>
        [HttpDelete]
        [Route("{timeseriesId}")]
        public IActionResult DeleteTimeseriesById(string timeseriesId)
        {
            var result = _influxDb.DeleteMeasurementById(timeseriesId).Result;
            if (result.Succeeded)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// This REST API handler returns the list of all timeseries
        /// </summary>
        /// <returns>List of timeseries</returns>
        [HttpGet]
        public IActionResult GetAllTimeseries()
        {
            var measurementsResult = _influxDb.ReadAllMeasurements().Result;
            {
                var ms = measurementsResult.ToArray();
                return Ok(ms.Select(t => t.Name).ToList());
            }
        }
    }
}