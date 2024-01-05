using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client.Core.Flux.Domain;

namespace sdLitica.TimeSeries.Services
{
    public interface IBucketService
    {
        Task<string> FindBucketNameById(string bucketId);
        Task<string> CreateBucket(string name, int retentionPeriod);
        Task DeleteBucketByName(string bucketId);
        Task UpdateBucketByName(string bucketId, int retentionPeriod);
    }
}
