using Newtonsoft.Json;
using sdLitica.Analytics;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    /// <summary>
    /// Event contains metadata for analytics operation. Used to send diagnostics info (status of operation, errors etc.)
    /// </summary>
    public class DiagnosticsResponse : Event
    {

        public DiagnosticsResponse() : base()
        {

        }

        public DiagnosticsResponse(AnalyticsOperationRequest operation) : base()
        {
            Operation = operation;
        }


        [JsonProperty]
        public AnalyticsOperationRequest Operation { get; set; }
    }
}
