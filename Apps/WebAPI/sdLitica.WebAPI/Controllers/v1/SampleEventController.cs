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
    [Route("api/v1/sampleevent")]
    //[Authorize]
    [AllowAnonymous]
    public class SampleEventController : BaseApiController
    {
        private readonly IEventBus _eventBus;

        public SampleEventController(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        /// <summary>
        /// This REST API handler returns the list of domain projects
        /// </summary>
        /// <returns>List of domain projects</returns>
        [HttpGet]
        public async Task<NoContentResult> Get([FromQuery] PaginationProperties pagination)
        {
            _eventBus.Publish(new TimeSeriesAnalysisEvent());

            await Task.CompletedTask;
            return new NoContentResult();
        }
    }
}