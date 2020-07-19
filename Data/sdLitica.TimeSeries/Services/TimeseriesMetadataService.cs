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
    /// <summary>
    /// This service provides operations with time-series metadata
    /// </summary>
    public class TimeseriesMetadataService : ITimeSeriesMetadataService
    {
        private readonly IAppSettings _appSettings;
        private readonly UserService _userService;
        private readonly TimeSeriesMetadataRepository _timeSeriesMetadataRepository;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="userService"></param>
        /// <param name="timeSeriesMetadataRepository"></param>
        public TimeseriesMetadataService(IAppSettings appSettings, UserService userService, TimeSeriesMetadataRepository timeSeriesMetadataRepository)
        {
            _appSettings = appSettings;
            _userService = userService;
            _timeSeriesMetadataRepository = timeSeriesMetadataRepository;
        }

        /// <summary>
        /// Creates new time-series metadata entity owned by user given by userId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TimeSeriesMetadata> AddTimeseriesMetadata(string name, string userId)
        {
            User user = _userService.GetUser(new Guid(userId));
            TimeSeriesMetadata t = TimeSeriesMetadata.Create(name, user);
            _timeSeriesMetadataRepository.Add(t);
            await _timeSeriesMetadataRepository.SaveChangesAsync();
            return t;
        }

        /// <summary>
        /// Updates time-series metadata with new name and description
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets list of time-series metadata objects owned by user given by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TimeSeriesMetadata> GetByUserId(string userId)
        {
            return _timeSeriesMetadataRepository.GetByUserId(new Guid(userId));
        }

        /// <summary>
        /// Gets untracked time-series metadata object given by guid. Useful for checking owner of time-series before actions on it.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public TimeSeriesMetadata GetTimeSeriesMetadata(string guid)
        {
            return _timeSeriesMetadataRepository.GetByIdReadonly(new Guid(guid));
        }

        /// <summary>
        /// Deletes time-series metadata object.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public async Task DeleteTimeSeriesMetadata(string guid)
        {
            TimeSeriesMetadata ts = _timeSeriesMetadataRepository.GetById(new Guid(guid));
            if (ts == null) throw new NotFoundException("this time-series is not found");

            _timeSeriesMetadataRepository.Delete(ts);
            await _timeSeriesMetadataRepository.SaveChangesAsync();
        }

    }
}
