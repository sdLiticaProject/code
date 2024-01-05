using System.Collections.Generic;
using System.Threading.Tasks;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.TimeSeries.Services
{
    public interface IBucketMetadataService
    {
        Task<BucketMetadata> AddBucketMetadata(string name, string description, string userId, int retentionPeriod, string influxId);
        
        List<BucketMetadata> GetByUserId(string userId);
        
        Task<BucketMetadata> UpdateBucketMetadata(string guid, string name, string description);
        
        Task<BucketMetadata> UpdateBucketMetadata(string guid, string name, string description, int retentionPeriod);
        
        Task DeleteBucketMetadata(string guid);
        
        BucketMetadata GetBucketMetadata(string guid);
    }
}