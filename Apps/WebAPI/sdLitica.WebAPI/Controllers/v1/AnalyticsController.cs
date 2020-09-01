﻿using System;
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
                OperationName = analyticsRequestModel.OperationName,
                TimeSeriesId = analyticsRequestModel.TimeSeriesId
            };

            _analyticsService.ExecuteOperation(analyticsOperation);
            analyticsRequestModel.Id = analyticsOperation.Id.ToString();

            Response.Headers.Add("Location", "api/v1/analytics/operations/"+analyticsOperation.Id.ToString());
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

        /// <summary>
        /// Get user's analytical operation by id (e.g. check status)
        /// </summary>
        /// <param name="userOperationId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("operations/{userOperationId}")]
        public IActionResult GetUserOperation([FromRoute] string userOperationId)
        {
            UserAnalyticsOperation userAnalyticsOperation = _analyticsService.GetUserAnalyticsOperation(userOperationId);
            if (userAnalyticsOperation == null) return NotFound();

            UserAnalyticsOperationModel model = new UserAnalyticsOperationModel()
            {
                Id = userAnalyticsOperation.Id.ToString(),
                OperationName = userAnalyticsOperation.OperationName,
                Status = userAnalyticsOperation.Status.ToString()
            };

            if (userAnalyticsOperation.Status.Equals(OperationStatus.Complete))
            {
                return Redirect("?");
            }
            return Ok(userAnalyticsOperation);
        }
    }
}
