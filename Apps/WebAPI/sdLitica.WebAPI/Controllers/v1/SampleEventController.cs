using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.TimeSeries.Services;
using sdLitica.WebAPI.Entities.Common;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is a sample conmtroller that requires authentication
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/sampleevent")]
    //[AllowAnonymous]
    //[Authorize]
    public class SampleEventController : BaseApiController
    {
        private readonly IEventBus _eventBus;
        private readonly ITimeSeriesService _timeSeriesService;

        public SampleEventController(IEventBus eventBus, ITimeSeriesService timeSeriesService)
        {
            _eventBus = eventBus;
            _timeSeriesService = timeSeriesService;
        }
        /// <summary>
        /// This REST API handler returns the list of domain projects
        /// </summary>
        /// <returns>List of domain projects</returns>
        [HttpGet]
        public async Task<NoContentResult> Get([FromQuery] PaginationProperties pagination)
        {
            Event tsEvent = new TimeSeriesAnalysisEvent();
            
            _eventBus.Publish(tsEvent);

            return new NoContentResult();
        }

        [HttpPost]
        public async Task<NoContentResult> Post([FromQuery] PaginationProperties pagination)
        {
            Task<string> task = _timeSeriesService.AddRandomTimeSeries();
            task.Wait();
            string seriesName = task.Result;
            Event tsEvent = new FSharpTimeSeriesAnalysisEvent();
            tsEvent.Name = seriesName;

            _eventBus.Publish(tsEvent);

            await Task.CompletedTask;

            return new NoContentResult();
        }
    }
}