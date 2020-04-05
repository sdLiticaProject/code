using Newtonsoft.Json;
using sdLitica.Analytics;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// This is sample event class used to illustrate for the team
    /// </summary>
    public class TimeSeriesAnalysisEvent : Event
    {

        public TimeSeriesAnalysisEvent() : base()
        {

        }

        public TimeSeriesAnalysisEvent(AnalyticsOperation operation) : base()
        {
            Operation = operation;
        }

        [JsonProperty]
        public AnalyticsOperation Operation { get; set; }
    }
}
