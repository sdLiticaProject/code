using Newtonsoft.Json;
using sdLitica.Events.Abstractions;
using sdLitica.Utils.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Integration
{
    public class AnalyticModuleRegistrationRequest : Event
    {
        public AnalyticModuleRegistrationRequest() : base()
        {

        }
        public AnalyticModuleRegistrationRequest(AnalyticsModuleRegistrationModel module) : base()
        {
            Module = module;
        }
        [JsonProperty]
        public AnalyticsModuleRegistrationModel Module { get; set; }
    }
}
