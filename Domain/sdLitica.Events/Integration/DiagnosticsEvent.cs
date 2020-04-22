using Newtonsoft.Json;
using sdLitica.Analytics;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// Event contains metadata for analytics operation. Used to send diagnostics info (status of operation, errors etc.)
    /// </summary>
    public class DiagnosticsEvent : Event
    {

        public DiagnosticsEvent() : base()
        {

        }

        public DiagnosticsEvent(AnalyticsOperation operation) : base()
        {
            Operation = operation;
        }


        [JsonProperty]
        public AnalyticsOperation Operation { get; set; }
    }
}
