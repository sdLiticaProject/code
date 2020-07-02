using System;
using System.Collections.Generic;
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
        Task<InfluxResult> DeleteMeasurementById(string measurementId);

    }
}