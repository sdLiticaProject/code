using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    public class AnalyticsOperation
    {
        public AnalyticsOperation()
        {

        }
        public string Name { get; set; }
        public Guid ModuleGuid { get; set; }
        public string QueueName { get; set; }
        public IList<string> QueueNames { get; set; } // decide what to keep
        public string Description { get; set; }
    }
}
