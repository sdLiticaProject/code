using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.WebAPI.Entities.Common;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is a sample conmtroller that requires authentication
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/analytics")]
    //[Authorize]
    public class AnalyticsController : BaseApiController
    {
        private readonly IEventBus _eventBus;

        public AnalyticsController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }


        /// <summary>
        /// This REST API handler returns result of some calculation over time-series given by timeseriesId
        /// </summary>
        /// <returns>Result of {operation} over time-series</returns>
        [HttpPost]
        [Route("mean/{timeseriesId}")] // todo: later expand to "{operation}/{timeseriesId}"
        public async Task<NoContentResult> Calculation()
        {
            throw new System.NotImplementedException();

            return new NoContentResult();
        }
    }
}