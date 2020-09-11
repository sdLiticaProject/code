using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Analytics
{
    /// <summary>
    /// This class represents analytical modules. 
    /// </summary>
    [Table("ANALYTICS_MODULES")]
    public class AnalyticsModule : Entity
    {
        public AnalyticsModule()
        {

        }


        /// <summary>
        /// Last time when this module sent alive-signal
        /// </summary>
        [Column("LAST_HEARD_TIME")]
        public DateTime LastHeardTime { get; set; }

        /// <summary>
        /// RabbitMQ queue to which this module is subscribed. 
        /// </summary>
        [Column("QUEUE_NAME")]
        public string QueueName { get; set; }

        /// <summary>
        /// Navigation-property for many-to-many relationship with AnalyticsOperation entity. 
        /// </summary>
        public IList<AnalyticsModulesOperations> AnalyticsModulesOperations { get; set; }
    }
}
