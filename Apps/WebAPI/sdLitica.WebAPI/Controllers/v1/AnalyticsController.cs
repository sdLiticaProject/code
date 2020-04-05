using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Analytics;
using sdLitica.AnalyticsManagementCore;

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
        /// This REST API handler returns result of some calculation given by OpName over time-series given by TsId
        /// </summary>
        /// <returns>Result of operation over time-series</returns>
        [HttpPost]
        [Route("calculate")] 
        public async Task<NoContentResult> Calculation([FromBody] AnalyticsOperation analyticsOperation)
        {
            _analyticsService.ExecuteOperation(analyticsOperation);
            return new NoContentResult();
        }
    }
}