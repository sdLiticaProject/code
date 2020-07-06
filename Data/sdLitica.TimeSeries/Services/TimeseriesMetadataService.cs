using sdLitica.Entities.Management;
using sdLitica.Entities.TimeSeries;
using sdLitica.Exceptions.Http;
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

        public async Task<TimeSeriesMetadata> AddTimeseriesMetadata(string name, string userId)
        {
            User user = _userService.GetUser(new Guid(userId));
            TimeSeriesMetadata t = TimeSeriesMetadata.Create(name, user);
            _timeSeriesMetadataRepository.Add(t);
            await _timeSeriesMetadataRepository.SaveChangesAsync();
            return t;
        }

        public async Task<TimeSeriesMetadata> UpdateTimeSeriesMetadata(string guid, string name, string description)
        {
            TimeSeriesMetadata t = _timeSeriesMetadataRepository.GetById(new Guid(guid));
            if (t == null)
            {
                throw new NotFoundException("this time-series is not found");
            }

            if (name == "") name = t.Name;
            if (description == "") description = t.Description; // maybe default value instead of ""

            t.Modify(name, description);
            await _timeSeriesMetadataRepository.SaveChangesAsync();
            return t;
        }

        public List<TimeSeriesMetadata> GetByUserId(string userId)
        {
            return _timeSeriesMetadataRepository.GetByUserId(new Guid(userId));
        }

        public TimeSeriesMetadata GetTimeSeriesMetadata(string guid)
        {
            return _timeSeriesMetadataRepository.GetById(new Guid(guid));
        }

        public async Task DeleteTimeSeriesMetadata(string guid)
        {
            TimeSeriesMetadata ts = _timeSeriesMetadataRepository.GetById(new Guid(guid));
            if (ts == null) throw new NotFoundException("this time-series is not found");

            _timeSeriesMetadataRepository.Delete(ts);
            await _timeSeriesMetadataRepository.SaveChangesAsync();
        }

    }
}
