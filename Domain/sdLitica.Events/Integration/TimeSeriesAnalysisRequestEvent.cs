using Newtonsoft.Json;
using sdLitica.Entities.Analytics;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// This is sample event class used to illustrate for the team
    /// </summary>
    public class TimeSeriesAnalysisRequestEvent : Event
    {

        public TimeSeriesAnalysisRequestEvent() : base()
        {

        }

        public TimeSeriesAnalysisRequestEvent(UserAnalyticsOperation operation) : base()
        {
            Operation = operation;
        }

        [JsonProperty]
        public UserAnalyticsOperation Operation { get; set; }
    }
}
