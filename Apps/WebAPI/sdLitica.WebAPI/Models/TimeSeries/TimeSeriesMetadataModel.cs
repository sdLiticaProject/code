using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using sdLitica.Entities.TimeSeries;

namespace sdLitica.WebAPI.Models.TimeSeries
{
    /// <summary>
    /// This class represents time-series metadata object
    /// </summary>
    public class TimeSeriesMetadataModel
    {
        /// <summary>
        /// Guid
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Name of time-series
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Description of time-series
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Date-time when this object was created
        /// </summary>
        public string DateCreated { get; set; }
        /// <summary>
        /// Date-time when this object was modified
        /// </summary>
        public string DateModified { get; set; }
        /// <summary>
        /// Id of user who owns this time-series object
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Id (name) of measurement in InfluxDB
        /// </summary>
        public string InfluxId { get; set; }

        /// <summary>
        /// no-args constructor
        /// </summary>
        public TimeSeriesMetadataModel() { }

        /// <summary>
        /// constructs from the object which this model represents
        /// </summary>
        /// <param name="timeSeriesMetadata"></param>
        public TimeSeriesMetadataModel(TimeSeriesMetadata timeSeriesMetadata)
        {
            Id = timeSeriesMetadata.Id.ToString();
            Name = timeSeriesMetadata.Name;
            Description = timeSeriesMetadata.Description;
            DateCreated = timeSeriesMetadata.DateCreated.ToString();
            DateModified = timeSeriesMetadata.DateModified.ToString();
            UserId = timeSeriesMetadata.UserId.ToString();
            InfluxId = timeSeriesMetadata.InfluxId.ToString();
        }
    }
}
