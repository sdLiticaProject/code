using sdLitica.Entities.Management;
using sdLitica.Entities.TimeSeries;
using sdLitica.PlatformCore;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.TimeSeries.Services
{
    public class TimeseriesMetadataService : ITimeSeriesMetadataService
    {
        private readonly IAppSettings _appSettings;
        private readonly UserService _userService;
        private readonly TimeSeriesMetadataRepository _timeSeriesMetadataRepository;
        public TimeseriesMetadataService(IAppSettings appSettings, UserService userService, TimeSeriesMetadataRepository timeSeriesMetadataRepository)
        {
            _appSettings = appSettings;
            _userService = userService;
            _timeSeriesMetadataRepository = timeSeriesMetadataRepository;
        }

        public async Task<string> AddTimeseriesMetadata(string name, string userId)
        {
            User user = _userService.GetUser(new Guid(userId));
            TimeSeriesMetadata t = TimeSeriesMetadata.Create(name, user);
            _timeSeriesMetadataRepository.Add(t);
            await _timeSeriesMetadataRepository.SaveChangesAsync();
            return t.Id.ToString();
        }

    }
}
