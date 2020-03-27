using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.ExampleDaemonManagement.Models
{
    public class TimeSeriesAnalysisEvent
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }


        [JsonProperty]
        public AnalysisOperation Operation { get; set; }

        public TimeSeriesAnalysisEvent()
        {

        }
    }
}
