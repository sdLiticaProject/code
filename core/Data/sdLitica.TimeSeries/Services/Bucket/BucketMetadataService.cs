using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper;
using sdLitica.Entities.Management;
using sdLitica.Entities.TimeSeries;
using sdLitica.Exceptions.Http;
using sdLitica.PlatformCore;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Abstractions;

namespace sdLitica.TimeSeries.Services
{

    public class BucketMetadataService : IBucketMetadataService
    {
        private readonly IAppSettings _appSettings;
        private readonly UserService _userService;
        private readonly BucketMetadataRepository _bucketMetadataRepository;
        
        public BucketMetadataService(IAppSettings appSettings, UserService userService, BucketMetadataRepository bucketMetadataRepository)
        {
            _appSettings = appSettings;
            _userService = userService;
            _bucketMetadataRepository = bucketMetadataRepository;
        }
        
        public async Task<BucketMetadata> AddBucketMetadata(string name, string description, string userId, int retentionPeriod, string influxId)
        {
            User user = _userService.GetUser(new Guid(userId));
            BucketMetadata t = BucketMetadata.Create(name, user, retentionPeriod, influxId, description);
            _bucketMetadataRepository.Add(t);
            await _bucketMetadataRepository.SaveChangesAsync();
            return t;
        }

        public List<BucketMetadata> GetByUserId(string userId)
        {
            return _bucketMetadataRepository.GetByUserId(new Guid(userId));
        }

        public async Task<BucketMetadata> UpdateBucketMetadata(string guid, string name, string description)
        {
            BucketMetadata t = _bucketMetadataRepository.GetById(new Guid(guid));
            if (t == null)
            {
                throw new NotFoundException("this bucket is not found");
            }

            if (name == "") name = t.Name;
            if (description == "") description = t.Description;

            t.Modify(name, description);
            await _bucketMetadataRepository.SaveChangesAsync();
            return t;
        }

        public async Task<BucketMetadata> UpdateBucketMetadata(string guid, string name, string description, int retentionPeriod)
        {
            BucketMetadata t = _bucketMetadataRepository.GetById(new Guid(guid));
            if (t == null)
            {
                throw new NotFoundException("this bucket is not found");
            }

            if (name == "") name = t.Name;
            if (description == "") description = t.Description;
            if (retentionPeriod < 0)
            {
                throw new InvalidRequestException("retention period is less than 0");
            }

            t.Modify(name, description, retentionPeriod);
            await _bucketMetadataRepository.SaveChangesAsync();
            return t;
        }

        public async Task DeleteBucketMetadata(string guid)
        {
            BucketMetadata b = _bucketMetadataRepository.GetById(new Guid(guid));
            if (b == null) throw new NotFoundException("this bucket is not found");

            _bucketMetadataRepository.Delete(b);
            await _bucketMetadataRepository.SaveChangesAsync();
        }

        public BucketMetadata GetBucketMetadata(string guid)
        {
            return _bucketMetadataRepository.GetById(new Guid(guid));
        }
    }
}