using System;
using Newtonsoft.Json;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    public class AnalysisResultRequestEvent: Event
    {
        [JsonProperty] public Guid AnalysisResultId { get; set; }

        public AnalysisResultRequestEvent(Guid analysisResultId)
        {
            AnalysisResultId = analysisResultId;
        }
    }
}