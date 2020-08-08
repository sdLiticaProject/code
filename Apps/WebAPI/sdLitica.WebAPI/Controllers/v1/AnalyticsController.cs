using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.AnalyticsManagementCore;
using sdLitica.Entities.Analytics;
using sdLitica.WebAPI.Models.Analytics;

namespace sdLitica.WebAPI.Controllers.v1
{
    /// <summary>
    /// This controller is a sample conmtroller for analytics operations
    /// </summary>
    [Produces("application/json")]
    [Route("api/v1/analytics")]
    //[Authorize]
    [AllowAnonymous]
    public class AnalyticsController : BaseApiController
    {
        private readonly AnalyticsService _analyticsService;

        public AnalyticsController(AnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }


        /// <summary>
        /// This REST API handler executes analytical operation. Then you (eventually) can access result of operation at [url]/[analyticsRequestModel.Id].txt
        /// </summary>
        /// <returns>Description of user's analytical operation</returns>
        [HttpPost]
        public async Task<IActionResult> Calculation ([FromBody] AnalyticsRequestModel analyticsRequestModel) //([FromBody] AnalyticsOperation analyticsOperation)
        {
            UserAnalyticsOperation analyticsOperation = new UserAnalyticsOperation()
            {
                Id = Guid.NewGuid(),
                OpName = analyticsRequestModel.OperationName,
                TimeSeriesId = analyticsRequestModel.TimeSeriesId
            };

            _analyticsService.ExecuteOperation(analyticsOperation);
            analyticsRequestModel.Id = analyticsOperation.Id.ToString();
            return Accepted(analyticsRequestModel);
        }

        /// <summary>
        /// Returns operations available to be executed by analytics modules
        /// </summary>
        /// <returns>list of operations</returns>
        [HttpGet]
        [Route("availableOperations")]
        public IActionResult GetAvailableOperations()
        {
            return Ok(_analyticsService.GetAvailableOperations());
        }
    }
}