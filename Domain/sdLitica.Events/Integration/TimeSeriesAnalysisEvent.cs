using Newtonsoft.Json;
using sdLitica.Analytics.Abstractions;
using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// This is sample event class used to illustrate for the team
    /// </summary>
    public class TimeSeriesAnalysisEvent : Event
    {
        [JsonProperty]
        IAnalyticsOperation Operation { get; set; }

        public TimeSeriesAnalysisEvent() : base()
        {

        }

        public TimeSeriesAnalysisEvent(IAnalyticsOperation operation) : base()
        {
            Operation = operation;
        }

    }
}
