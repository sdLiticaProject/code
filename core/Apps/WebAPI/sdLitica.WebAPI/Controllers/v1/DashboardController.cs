using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client.Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sdLitica.Entities.TimeSeries;
using sdLitica.Models.Dashboard;
using sdLitica.TimeSeries.Services;
using sdLitica.TimeSeries.Services.Dashboard;
using sdLitica.WebAPI.Entities.Common;

namespace sdLitica.WebAPI.Controllers.v1
{

    [Route("api/v1/dashboard")]
    [Authorize]
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService _dashboardsService;
        private readonly IWidgetService _widgetService;

        public DashboardController(IDashboardService dashboardService, IWidgetService widgetService)
        {
            _dashboardsService = dashboardService;
            _widgetService = widgetService;
        }
        
        [HttpPost]
        public IActionResult CreateDashboard([FromBody] DashboardMetadataModel dashboardModel)
        {
            Task<DashboardMetadata> t = _dashboardsService.CreateDashboardMetadata(dashboardModel.Title, dashboardModel.Description, UserId);
            return Ok(new DashboardMetadataModel(t.Result));
        }
        
        [HttpPost]
        [Route("{dashboardMetadataId}")]
        public IActionResult UpdateDashboard([FromBody] DashboardMetadataModel dashboardModel, [FromRoute] string dashboardMetadataId)
        {
            DashboardMetadata dashboardMetadata = _dashboardsService.getDashboardMetadata(dashboardMetadataId);
            if (dashboardMetadata == null || !dashboardMetadata.UserId.ToString().Equals(UserId))
            {
                return NotFound("this user does not have this dashboard");
            }
            
            Task<DashboardMetadata> t = _dashboardsService.UpdateDashboardMetadata(dashboardMetadataId, dashboardModel.Title, dashboardModel.Description);
            return Ok(new DashboardMetadataModel(t.Result));
        }
        
        [HttpDelete]
        [Route("{dashboardMetadataId}")]
        public IActionResult DeleteDashboard([FromRoute] string dashboardMetadataId)
        {
            DashboardMetadata dashboardMetadata = _dashboardsService.getDashboardMetadata(dashboardMetadataId);
            if (dashboardMetadata == null || !dashboardMetadata.UserId.ToString().Equals(UserId))
            {
                return NotFound("this user does not have this dashboard");
            }
            
            _dashboardsService.DeleteDashboardtMetadata(dashboardMetadataId).Wait();
            return Ok();
        }
        
        [HttpGet]
        public IActionResult GetAllDashboards()
        {
            List<DashboardMetadata> t = _dashboardsService.GetByUserId(UserId);
            List<DashboardMetadataModel> list = new List<DashboardMetadataModel>();
            t.ForEach(e => list.Add(new DashboardMetadataModel(e)));
            return Ok(list);
        }
        
        [HttpGet]
        [Route("{dashboardMetadataId}")]
        public IActionResult GetDashboard([FromRoute] string dashboardMetadataId)
        {
            var t = _dashboardsService.getDashboardMetadata(dashboardMetadataId);
            if (t == null || !t.UserId.ToString().Equals(UserId))
            {
                return NotFound("this user does not have this dashboard");
            }

            return Ok(new DashboardMetadataModel(t));
        }

    }
}