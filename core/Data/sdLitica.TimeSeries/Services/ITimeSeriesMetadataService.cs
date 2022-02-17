using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.TimeSeries.Services
{
    /// <summary>
    /// Interface to describe operations with time-series metadata
    /// </summary>
    public interface ITimeSeriesMetadataService
    {
        /// <summary>
        /// Create new time-series metadata object
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TimeSeriesMetadata> AddTimeseriesMetadata(string name, string description, string userId);

        /// <summary>
        /// Get time-series metadata object by user-id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<TimeSeriesMetadata> GetByUserId(string userId);

        /// <summary>
        /// Update time-series metadata object given by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<TimeSeriesMetadata> UpdateTimeSeriesMetadata(string guid, string name, string description);

        /// <summary>
        /// Update time-series metadata object given by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        Task<TimeSeriesMetadata> UpdateTimeSeriesMetadataColumns(string guid, IReadOnlyCollection<string> columns);

        /// <summary>
        /// Delete time-series metadata object given by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        Task DeleteTimeSeriesMetadata(string guid);

        /// <summary>
        /// Get time-series metadata object given by guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        TimeSeriesMetadata GetTimeSeriesMetadata(string guid);
    }
}
