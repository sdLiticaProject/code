using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Analytics
{
    /// <summary>
    /// This class represents available analytical operation. 
    /// </summary>
    [Table("ANALYTICS_OPERATIONS")]
    public class AnalyticsOperation : Entity
    {
        public AnalyticsOperation()
        {

        }

        /// <summary>
        /// RabbitMQ routing key to this operation of any module (*.operation)
        /// </summary>
        [Column("ROUTING_KEY")]
        public string RoutingKey { get; set; }

        /// <summary>
        /// Unique (but not yet) name of analytical operation
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// Navigation-property for many-to-many relationship with AnalyticsModule entity. 
        /// </summary>
        public IList<AnalyticsModulesOperations> AnalyticsModulesOperations { get; set; }

        /// <summary>
        /// Description of analytical operation.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
