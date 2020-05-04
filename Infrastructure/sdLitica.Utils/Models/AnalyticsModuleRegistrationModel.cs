using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Utils.Models
{
    class AnalyticsModuleRegistrationModel
    {
        public AnalyticsModuleRegistrationModel() { }
        public Guid ModuleGuid { get; set; }
        public string QueueName { get; set; }
        public IList<AnalyticsOperationModel> Operations { get; set; }
    }
}
