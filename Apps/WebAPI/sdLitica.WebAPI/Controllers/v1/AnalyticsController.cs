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
        /// This REST API handler returns result of some calculation given by OperationName over time-series given by TimeSeriesId
        /// </summary>
        /// <returns>Result of operation over time-series</returns>
        [HttpPost]
        public async Task<NoContentResult> Calculation ([FromBody] AnalyticsRequestModel analyticsRequestModel) //([FromBody] AnalyticsOperation analyticsOperation)
        {
            UserAnalyticsOperation analyticsOperation = new UserAnalyticsOperation()
            {
                Id = Guid.NewGuid(),
                OperationName = analyticsRequestModel.OperationName,
                TimeSeriesId = analyticsRequestModel.TimeSeriesId
            };

            _analyticsService.ExecuteOperation(analyticsOperation);
            return new NoContentResult();
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