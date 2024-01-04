using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using sdLitica.Entities.Abstractions;
using sdLitica.Entities.Management;

namespace sdLitica.Entities.TimeSeries
{
    /// <summary>
    /// This class represents metadata of time-series
    /// </summary>
    [Table("TIMESERIES_METADATA")]
    public class TimeSeriesMetadata: Entity
    {
        /// <summary>
        /// Creates a time-series with no parameters
        /// </summary>
        protected TimeSeriesMetadata()
        {
        }

        /// <summary>
        /// User-defined name of time-series
        /// </summary>
        [Column("NAME")]
        public string Name { get; protected set; }
        /// <summary>
        /// User-defined description of time-series
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; protected set; }

        [Column("TYPE")] public string Type { get; protected set; }

        /// <summary>
        /// Name of time-series in InfluxDB
        /// </summary>
        [Column("INFLUX_ID")]
        public Guid InfluxId { get; protected set; }
        /// <summary>
        /// Date-time of creation
        /// </summary>
        [Column("DATE_CREATED")]
        public DateTime DateCreated { get; protected set; }
        /// <summary>
        /// Date-time of last modification
        /// </summary>
        [Column("DATE_MODIFIED")]
        public DateTime DateModified { get; protected set; }
        /// <summary>
        /// Id of user owning the time-series
        /// </summary>
        [Column("BUCKET_ID")]
        public Guid BucketId { get; protected set; }
        
        
        public BucketMetadata Bucket { get; protected set; }

        /// <summary>
        /// Length of time-series, updates when loading the time-series
        /// </summary>
        [Column("ROWS_COUNT")]
        public int RowsCount { get; protected set; }
        
        /// <summary>
        /// Amount of columns, updates when loading the time-series
        /// </summary>
        [Column("COLUMNS_COUNT")]
        public int ColumnsCount { get; protected set; }
        
        /// <summary>
        /// Comma-separated list of columns' names, updates when loading the time-series
        /// </summary>
        [Column("COLUMNS", TypeName = "json")]
        public string Columns { get; protected set; }
        
        /// <summary>
        /// Comma-separated list of tags' names, updates when loading the time-series
        /// </summary>
        [Column("TAGS", TypeName = "json")] 
        public string Tags { get; protected set; }

        /// <summary>
        /// Modify time-series
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void Modify(string name, string description)
        {
            Name = name;
            Description = description;
            DateModified = DateTime.Now;
        }
        
        public void Modify(string name, string description, int rowsCount, int columnsCount, string columns, string tags)
        {
            Name = name;
            Description = description;
            DateModified = DateTime.Now;
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
            Columns = columns;
            Tags = tags;
        }

        /// <summary>
        /// Create new time-series metadata entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static TimeSeriesMetadata Create(string name, BucketMetadata bucket, string type, string description = "")
        {
            TimeSeriesMetadata timeSeriesMetadata = new TimeSeriesMetadata()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                Bucket = bucket,
                Type = type,
                InfluxId = Guid.NewGuid(),
                ColumnsCount = 0,
                RowsCount = 0,
                Columns = "[]",
                Tags = "{}"
            };
            return timeSeriesMetadata;
        }
    }
}
