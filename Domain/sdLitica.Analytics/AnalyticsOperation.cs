using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics
{
    /// <summary>
    /// This class represents available analytical operation. 
    /// </summary>
    public class AnalyticsOperation : Entity
    {
        public AnalyticsOperation()
        {

        }
        public Guid Id { get; set; } // temporary

        /// <summary>
        /// Unique (but not yet) name of analytical operation
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigation-property for many-to-many relationship with AnalyticsModule entity. 
        /// </summary>
        public IList<ModulesOperations> ModulesOperations { get; set; }

        /// <summary>
        /// Description of analytical operation.
        /// </summary>
        public string Description { get; set; }
    }
}
