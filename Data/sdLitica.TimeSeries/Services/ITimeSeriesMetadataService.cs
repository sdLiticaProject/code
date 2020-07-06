using sdLitica.Entities.TimeSeries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.TimeSeries.Services
{
    public interface ITimeSeriesMetadataService
    {
        Task<TimeSeriesMetadata> AddTimeseriesMetadata(string name, string userId);
        List<TimeSeriesMetadata> GetByUserId(string userId);
        Task<TimeSeriesMetadata> UpdateTimeSeriesMetadata(string guid, string name, string description);
        Task DeleteTimeSeriesMetadata(string guid);
        TimeSeriesMetadata GetTimeSeriesMetadata(string guid);
    }
}
