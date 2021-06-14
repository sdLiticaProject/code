using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sdLitica.AnalysisResults.Model;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    public class NewAnalysisResultEvent: Event
    {
        [JsonProperty] public Guid TimeSeriesId { get; init; }
        [JsonProperty] public AnalysisResultType Type { get; init; }
        // TODO: it's may be generic type if mongodb driver are able to [de]serialize it
        [JsonProperty] public JObject Result { get; init; }

        public NewAnalysisResultEvent(Guid timeSeriesId, AnalysisResultType type, JObject result)
        {
            TimeSeriesId = timeSeriesId;
            Type = type;
            Result = result;
        }
    }
}