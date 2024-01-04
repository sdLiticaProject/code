using System;
using System.ComponentModel.DataAnnotations.Schema;
using sdLitica.Entities.Abstractions;
using sdLitica.Entities.Management;

namespace sdLitica.Entities.TimeSeries
{
    [Table("WIDGETS")]
    public class WidgetMetadata : Entity
    {
        protected WidgetMetadata()
        {
        }
        
        [Column("TITLE")]
        public string Title { get; protected set; }

        [Column("DESCRIPTION")]
        public string Description { get; protected set; }

        [Column("DASHBOARD_ID")]
        public Guid DashboardId { get; protected set; }
        
        /// <summary>
        /// Navigation property for the dashboard
        /// </summary>
        public DashboardMetadata Dashboard { get; protected set; }

        [Column("TYPE")] 
        public string Type { get; set; }
        
        [Column("TIMESERIES_ID")]
        public Guid TimeseriesId { get; protected set; }
        
        /// <summary>
        /// Navigation property for the time-series
        /// </summary>
        public TimeSeriesMetadata TimeSeriesMetadata { get; protected set; }

        [Column("ARGUMENTS")]
        public string Arguments { get; set; }
        
        public void Modify(string arguments)
        {
            Arguments = arguments;
        }

        /// <summary>
        /// Create new bucket metadata entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static WidgetMetadata Create(string title, DashboardMetadata dbMetadata, string type, TimeSeriesMetadata ts, string arguments, string description = "")
        {
            WidgetMetadata widgetMetadata = new WidgetMetadata()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                Dashboard = dbMetadata,
                Type = type,
                TimeSeriesMetadata = ts,
                Arguments = arguments
            };
            return widgetMetadata;
        }
    }
}