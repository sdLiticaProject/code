using Newtonsoft.Json;
using sdLitica.Analytics.Abstractions;
using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Integration
{
    public class DiagnosticsEvent : Event
    {
        [JsonProperty]
        public AnalyticsOperation Operation { get; set; }
        //IAnalyticsOperation Operation { get; set; }

        public DiagnosticsEvent() : base()
        {

        }

        public DiagnosticsEvent(AnalyticsOperation operation) : base()
        {
            Operation = operation;
        }
    }
}
