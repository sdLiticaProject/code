using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics
{
    /// <summary>
    /// Linking entity to support many-to-many relationship for AnalyticsModule and AnalyticsOperation.
    /// </summary>
    public class ModulesOperations : Entity
    {
        public ModulesOperations()
        {
            Id = Guid.NewGuid();
        }

        /// <summary>
        /// Id of related AnalyticsModule
        /// </summary>
        public Guid AnalyticsModuleId { get; set; }

        /// <summary>
        /// Navigation-property for related AnalyticsModule
        /// </summary>
        public AnalyticsModule AnalyticsModule { get; set; }

        /// <summary>
        /// Id of related AnalyticsOperation
        /// </summary>
        public Guid AnalyticsOperationId { get; set; }

        /// <summary>
        /// Navigation-property for related AnalyticsOperation. 
        /// </summary>
        public AnalyticsOperation AnalyticsOperation { get; set; }
    }
}
