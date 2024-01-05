using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
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
        [Required]
        public string Name { get; set; }
        
        public string Type { get; set; }
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
        public string BucketId { get; set; }
        /// <summary>
        /// Id (name) of measurement in InfluxDB
        /// </summary>
        public string InfluxId { get; set; }
        
        public int RowsCount { get; set; }
        
        public int ColumnsCount { get; set; }
        
        public HashSet<string> Columns { get; set; }
        
        public Dictionary<string, HashSet<string>> Tags { get; set; }

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
            BucketId = timeSeriesMetadata.BucketId.ToString();
            Type = timeSeriesMetadata.Type;
            InfluxId = timeSeriesMetadata.InfluxId.ToString();
            RowsCount = timeSeriesMetadata.RowsCount;
            ColumnsCount = timeSeriesMetadata.ColumnsCount;
            Columns = JsonSerializer.Deserialize<HashSet<string>>(timeSeriesMetadata.Columns);
            Tags = JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(timeSeriesMetadata.Tags);
        }
    }
}
