using sdLitica.Entities.Abstractions;
using sdLitica.Entities.Management;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Entities.TimeSeries
{
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
        public string Name { get; protected set; }
        /// <summary>
        /// User-defined description of time-series
        /// </summary>
        public string Description { get; protected set; }
        /// <summary>
        /// Name of time-series in InfluxDB
        /// </summary>
        public Guid InfluxId { get; protected set; }
        /// <summary>
        /// Date-time of creation
        /// </summary>
        public DateTime DateCreated { get; protected set; }
        /// <summary>
        /// Date-time of last modification
        /// </summary>
        public DateTime DateModified { get; protected set; }
        /// <summary>
        /// Id of user owning the time-series
        /// </summary>
        public Guid UserId { get; protected set; }
        /// <summary>
        /// Navigation property for user owning the time-series
        /// </summary>
        public User User { get; protected set; }

        /// <summary>
        /// Length of time-series, updates when loading the time-series
        /// </summary>
        public int RowsCount { get; protected set; }
        /// <summary>
        /// Amount of columns, updates when loading the time-series
        /// </summary>
        public int ColumnsCount { get; protected set; }
        /// <summary>
        /// Comma-separated list of columns' names, updates when loading the time-series
        /// </summary>
        public string Columns { get; protected set; }

        /// <summary>
        /// Modify time-series
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="rowsCount"></param>
        /// <param name="columnsCount"></param>
        /// <param name="columns"></param>
        public void Modify(string name, string description, int rowsCount, int columnsCount, string columns)
        {
            Name = name;
            Description = description;
            RowsCount = rowsCount;
            ColumnsCount = columnsCount;
            Columns = columns;
            DateModified = DateTime.Now;
        }

        /// <summary>
        /// Create new time-series metadata entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static TimeSeriesMetadata Create(string name, User owner, string description = "")
        {
            TimeSeriesMetadata timeSeriesMetadata = new TimeSeriesMetadata()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                User = owner,
                InfluxId = Guid.NewGuid()                
            };
            return timeSeriesMetadata;
        }
    }
}
