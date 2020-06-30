using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.TimeSeries.Services
{
    public interface ITimeSeriesMetadataService
    {
        Task<string> AddTimeseriesMetadata(string name, string userId);
    }
}
