using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using sdLitica.Entities.Management;
using sdLitica.Entities.TimeSeries;
using sdLitica.Exceptions.Http;
using sdLitica.PlatformCore;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Abstractions;

namespace sdLitica.TimeSeries.Services.Dashboard
{

    public class DashBoardService : IDashboardService
    {
        private readonly IAppSettings _appSettings;
        private readonly UserService _userService;
        private readonly DashBoardRepository _dashBoardRepository;
        
        public DashBoardService(IAppSettings appSettings, UserService userService, DashBoardRepository dashBoardRepository)
        {
            _appSettings = appSettings;
            _userService = userService;
            _dashBoardRepository = dashBoardRepository;
        }

        public async Task<DashboardMetadata> CreateDashboardMetadata(string title, string description, string userId)
        {
            User user = _userService.GetUser(new Guid(userId));
            DashboardMetadata t = DashboardMetadata.Create(title, user, description);
            _dashBoardRepository.Add(t);
            await _dashBoardRepository.SaveChangesAsync();
            return t;
        }

        public List<DashboardMetadata> GetByUserId(string userId)
        {
            return _dashBoardRepository.GetByUserId(new Guid(userId));
        }

        public async Task<DashboardMetadata> UpdateDashboardMetadata(string guid, string title, string description)
        {
            DashboardMetadata t = _dashBoardRepository.GetById(new Guid(guid));
            if (t == null)
            {
                throw new NotFoundException("this dashboard is not found");
            }

            if (title == "") title = t.Title;
            if (description == "") description = t.Description;

            t.Modify(title, description);
            await _dashBoardRepository.SaveChangesAsync();
            return t;
        }

        public async Task DeleteDashboardtMetadata(string guid)
        {
            DashboardMetadata t = _dashBoardRepository.GetById(new Guid(guid));
            if (t == null) throw new NotFoundException("this dashboard is not found");

            _dashBoardRepository.Delete(t);
            await _dashBoardRepository.SaveChangesAsync();
        }

        public DashboardMetadata getDashboardMetadata(string guid)
        {
            return _dashBoardRepository.GetById(new Guid(guid));
        }
    }
}