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
        public string Description { get; set; }
    }
}
