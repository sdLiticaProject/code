using sdLitica.Analytics.Abstractions;
using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    class AnalyticsOperationEvent: Event
    {
        IAnalyticsOperation Operation { get; set; }

        public AnalyticsOperationEvent(IAnalyticsOperation operation) : base()
        {
            
            Operation = operation;
        }
    }
}
