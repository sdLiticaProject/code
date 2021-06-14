using System;
using Newtonsoft.Json.Linq;

namespace sdLitica.AnalysisResults.Model
{
    public class AnalysisResult
    {
        public Guid Id { get; init; }
        public Guid TimeSeriesId { get; init; }
        public DateTime DateCreated { get; init; }
        public AnalysisResultType Type { get; init; }
        // TODO: it's may be generic type if mongodb driver are able to [de]serialize it
        public JObject Result { get; init; }
    }
}