using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.WebAPI.Services;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is used to work with user timeserieses
    /// </summary>
    [Route("api/v1")]
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
        /// This REST API handler returns the list of all measurements in influxDB
        /// </summary>
        /// <returns>204 if measurements not found, instead - list of measurements</returns>
        [Route("timeseries/measurement/all")]
        [HttpGet]
        public IActionResult GetAllMeasurements()
        {
            var measurementsResult = _influxDb.ReadAllMeasurements().Result;
            if (measurementsResult.Count != 0)
            {
                var ms = measurementsResult.ToArray();
                return Ok(ms.Select(t => t.Name).ToList());
            }
            else
            {
                return NoContent();
            }
        }
    }
}