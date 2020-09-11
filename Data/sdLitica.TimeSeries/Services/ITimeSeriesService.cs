using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.TimeSeries.Services
{
    public interface ITimeSeriesService
    {
        Task<InfluxResult> CreateUser(string username, string password);
        Task<List<MeasurementRow>> ReadAllMeasurements();

        Task<string> AddRandomTimeSeries();
        Task<string> AddRandomTimeSeries(string measurementId);
        
        Task<InfluxResult<DynamicInfluxRow>> ReadMeasurementById(string measurementId);
        Task<InfluxResult<DynamicInfluxRow>> ReadMeasurementById(string measurementId, string from, string to, string step);
        Task<InfluxResult> DeleteMeasurementById(string measurementId);

        Task<string> UploadDataFromCsv(string measurementId, List<string> file);

    }
}