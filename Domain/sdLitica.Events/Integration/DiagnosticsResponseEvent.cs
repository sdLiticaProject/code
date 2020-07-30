using Newtonsoft.Json;
using sdLitica.Entities.Analytics;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// Event contains metadata for analytics operation. Used to send diagnostics info (status of operation, errors etc.)
    /// </summary>
    public class DiagnosticsResponseEvent : Event
    {

        public DiagnosticsResponseEvent() : base()
        {

        }

        public DiagnosticsResponseEvent(UserAnalyticsOperation operation) : base()
        {
            Operation = operation;
        }


        [JsonProperty]
        public UserAnalyticsOperation Operation { get; set; }
    }
}
