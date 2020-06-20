using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics
{
    public class ModulesOperations : Entity
    {
        public ModulesOperations()
        {
            Id = Guid.NewGuid();
        }

        public Guid AnalyticsModuleId { get; set; }
        public AnalyticsModule AnalyticsModule { get; set; }
        public Guid AnalyticsOperationId { get; set; }
        public AnalyticsOperation AnalyticsOperation { get; set; }
    }
}
