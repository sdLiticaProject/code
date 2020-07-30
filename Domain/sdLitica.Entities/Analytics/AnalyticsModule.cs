using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Entities.Analytics
{
    /// <summary>
    /// This class represents analytical modules. 
    /// </summary>
    public class AnalyticsModule : Entity
    {
        public AnalyticsModule()
        {

        }

        public Guid Id { get; set; } //temporary

        /// <summary>
        /// Last time when this module sent alive-signal
        /// </summary>
        public DateTime LastHeardTime { get; set; }

        /// <summary>
        /// RabbitMQ queue to which this module is subscribed. 
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// Navigation-property for many-to-many relationship with AnalyticsOperation entity. 
        /// </summary>
        public IList<ModulesOperations> ModulesOperations { get; set; }
    }
}
