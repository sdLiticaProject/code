using Newtonsoft.Json;
using sdLitica.Analytics;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// This is sample event class used to illustrate for the team
    /// </summary>
    public class TimeSeriesAnalysisRequest : Event
    {

        public TimeSeriesAnalysisRequest() : base()
        {

        }

        public TimeSeriesAnalysisRequest(AnalyticsOperationRequest operation) : base()
        {
            Operation = operation;
        }

        [JsonProperty]
        public AnalyticsOperationRequest Operation { get; set; }
    }
}
