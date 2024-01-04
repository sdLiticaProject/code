using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;

namespace sdLitica.TimeSeries.Services
{
    public interface ITimeSeriesService
    {
        Task<User> CreateUser(string username, string password);

        Task<string> AddRandomTimeSeries(string bucketId);
        Task<string> AddRandomTimeSeries(string measurementId, string bucketId);

        Task<List<FluxTable>> ReadMeasurementById(string measurementId, string bucketId);
        Task<List<FluxTable>> ReadMeasurementById(string measurementId, string bucketId, string from, string to, string step, string agrFn);
        
        Task<string> CreateFileCSV(string measurementId, string bucketId);
        
        Task DeleteMeasurementById(string measurementId, string bucketId);

        Task<(int columnsCount, int rowCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)>
            UploadDataFromCsv(string measurementId, string bucketId, List<string> file);

        Task<(int columnsCount, int rowCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)>
            UploadDataFromJTs(string measurementId, string bucketId, TimeSeriesJTSEntity jsonTsData);

        Task<(int columnsCount, int rowCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)>
            UploadDataFromWebSocket(string measurementId, string bucketId, TimeSeriesWebSocketEntity jsonTsData);

    }
}