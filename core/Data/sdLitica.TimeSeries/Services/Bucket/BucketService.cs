using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.TimeSeries.Services
{

    public class BucketService : IBucketService
    {
        private TimeSeriesSettings TimeSeriesSettings { set; get; }

        private readonly InfluxDBClient _influxClient;
        private readonly IAppSettings _settings;

        public BucketService(IAppSettings settings)
        {
            TimeSeriesSettings = settings.TimeSeriesSettings;
            _influxClient = InfluxDBClientFactory.Create(TimeSeriesSettings.InfluxHostName, TimeSeriesSettings.InfluxToken);
            _settings = settings;
        }

        public async Task<string> FindBucketNameById(string bucketId)
        {
            return _influxClient.GetBucketsApi().FindBucketByIdAsync(bucketId).Result.Name;
        }

        public async Task<string> CreateBucket(string name, int retentionPeriod)
        {
            var retention =  new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, retentionPeriod);
            var bucket = await _influxClient.GetBucketsApi().CreateBucketAsync(name, retention, TimeSeriesSettings.InfluxOrgId);
            return bucket.Name;
        }

        public async Task DeleteBucketByName(string bucketId)
        {
            var bucket = _influxClient.GetBucketsApi().FindBucketByNameAsync(bucketId).Result;
            await _influxClient.GetBucketsApi().DeleteBucketAsync(bucket);
        }

        public async Task UpdateBucketByName(string bucketId, int retentionPeriod)
        {
            var bucket = _influxClient.GetBucketsApi().FindBucketByNameAsync(bucketId).Result;
            if (bucket != null)
            {
                bucket.RetentionRules.Clear();
                bucket.RetentionRules.Add(new BucketRetentionRules(BucketRetentionRules.TypeEnum.Expire, retentionPeriod));
                await _influxClient.GetBucketsApi().UpdateBucketAsync(bucket);
            }
        }
    }
}