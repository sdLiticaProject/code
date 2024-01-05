using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace sdLitica.TimeSeries.Services
{

    public class TimeSeriesWebSocketEntity
    {
        public List<string> Time { get; set; }
        public Dictionary<string, List<string>> Fields { get; set; }
        public Dictionary<string, List<string>> Tags { get; set; }
        
        public TimeSeriesWebSocketEntity()
        {
        }

        public TimeSeriesWebSocketEntity(TimeSeriesWebSocketEntity timeSeriesWebSocketEntity)
        {
            Time = timeSeriesWebSocketEntity.Time;
            Fields = timeSeriesWebSocketEntity.Fields;
            Tags = timeSeriesWebSocketEntity.Tags;
        }
    }
}