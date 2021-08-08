using Newtonsoft.Json;
using sdLitica.AnalysisResults.Model;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    public class AnalysisResultResponseEvent: Event
    {
        [JsonProperty] public AnalysisResult Result { get; set; }

        public AnalysisResultResponseEvent(AnalysisResult result)
        {
            Result = result;
        }
    }
}