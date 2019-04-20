using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.WebAPI.Services
{
    public interface IInfluxDB
    {
        Task<InfluxResult> CreateUser(string username, string password);
        Task<List<MeasurementRow>> ReadAllMeasurements();

        Task<string> AddRandomTimeseries();
        
        Task<InfluxResult<DynamicInfluxRow>> ReadMeasurementById(string measurementId);
        Task<InfluxResult> DeleteMeasurementById(string measurementId);

    }
}