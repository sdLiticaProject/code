using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using sdLitica.Entities.Management;
using sdLitica.Entities.TimeSeries;
using sdLitica.Exceptions.Http;
using sdLitica.PlatformCore;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Abstractions;

namespace sdLitica.TimeSeries.Services
{
    /// <summary>
    /// This service provides operations with time-series metadata
    /// </summary>
    public class TimeseriesMetadataService : ITimeSeriesMetadataService
    {
        private readonly IAppSettings _appSettings;
        private readonly IBucketMetadataService _bucketMetadataService;
        private readonly BucketMetadataRepository _bucketMetadataRepository;
        private readonly TimeSeriesMetadataRepository _timeSeriesMetadataRepository;

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="appSettings"></param>
        /// <param name="userService"></param>
        /// <param name="timeSeriesMetadataRepository"></param>
        public TimeseriesMetadataService(IAppSettings appSettings, IBucketMetadataService bucketMetadataService, TimeSeriesMetadataRepository timeSeriesMetadataRepository, BucketMetadataRepository bucketMetadataRepository)
        {
            _appSettings = appSettings;
            _bucketMetadataService = bucketMetadataService;
            _timeSeriesMetadataRepository = timeSeriesMetadataRepository;
            _bucketMetadataRepository = bucketMetadataRepository;
        }

        /// <summary>
        /// Creates new time-series metadata entity owned by user given by userId
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<TimeSeriesMetadata> AddTimeseriesMetadata(string name, string description, string bucketId, string type)
        {
            BucketMetadata bucketMetadata = _bucketMetadataService.GetBucketMetadata(bucketId);
            TimeSeriesMetadata t = TimeSeriesMetadata.Create(name, bucketMetadata, type,description);
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
        
        public async Task<TimeSeriesMetadata> UpdateTimeSeriesMetadata(string guid, string name, string description,
            int rowsCount, int columnsCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)
        {
            TimeSeriesMetadata t = _timeSeriesMetadataRepository.GetById(new Guid(guid));
            if (t == null)
            {
                throw new NotFoundException("this time-series is not found");
            }

            if (name == "") name = t.Name;
            if (description == "") description = t.Description; // maybe default value instead of ""

            t.Modify(name, description, rowsCount, columnsCount, JsonSerializer.Serialize(columns), JsonSerializer.Serialize(tags));
            await _timeSeriesMetadataRepository.SaveChangesAsync();
            return t;
        }

        
        public List<TimeSeriesMetadata> GetByBucketId(string bucketId)
        {
            return _timeSeriesMetadataRepository.GetByBucketId(new Guid(bucketId));
        }

        /// <summary>
        /// Gets untracked time-series metadata object given by guid. Useful for checking owner of time-series before actions on it.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public TimeSeriesMetadata GetTimeSeriesMetadata(string guid)
        {
            return _timeSeriesMetadataRepository.GetById(new Guid(guid));
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
