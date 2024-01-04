using System;
using System.ComponentModel.DataAnnotations.Schema;
using sdLitica.Entities.Abstractions;
using sdLitica.Entities.Management;

namespace sdLitica.Entities.TimeSeries
{
    [Table("DASHBOARDS")]
    public class DashboardMetadata : Entity
    {

        protected DashboardMetadata()
        {
        }
        
        [Column("TITLE")]
        public string Title { get; protected set; }

        [Column("DESCRIPTION")]
        public string Description { get; protected set; }

        [Column("USER_ID")]
        public Guid UserId { get; protected set; }
        
        /// <summary>
        /// Navigation property for user owning the dashboard
        /// </summary>
        public User User { get; protected set; }

        public void Modify(string title, string description)
        {
            Title = title;
            Description = description;
        }

        /// <summary>
        /// Create new bucket metadata entity
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static DashboardMetadata Create(string title, User owner, string description = "")
        {
            DashboardMetadata dashboardMetadata = new DashboardMetadata()
            {
                Id = Guid.NewGuid(),
                Title = title,
                Description = description,
                User = owner,
            };
            return dashboardMetadata;
        }
    }
}