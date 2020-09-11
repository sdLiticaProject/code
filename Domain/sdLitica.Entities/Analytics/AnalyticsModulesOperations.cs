using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using sdLitica.Entities.Abstractions;

namespace sdLitica.Entities.Analytics
{
    /// <summary>
    /// Linking entity to support many-to-many relationship for AnalyticsModule and AnalyticsOperation.
    /// </summary>
    [Table("ANALYTICS_MODULES_OPERATIONS")]
    public class AnalyticsModulesOperations : Entity
    {
        public AnalyticsModulesOperations()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Id of related AnalyticsModule
        /// </summary>
        [Column("ANALYTICS_MODULE_ID")]
        public Guid AnalyticsModuleId { get; set; }

        /// <summary>
        /// Navigation-property for related AnalyticsModule
        /// </summary>
        public AnalyticsModule AnalyticsModule { get; set; }

        /// <summary>
        /// Id of related AnalyticsOperation
        /// </summary>
        [Column("ANALYTICS_OPERATION_ID")]
        public Guid AnalyticsOperationId { get; set; }

        /// <summary>
        /// Navigation-property for related AnalyticsOperation. 
        /// </summary>
        public AnalyticsOperation AnalyticsOperation { get; set; }
    }
}
