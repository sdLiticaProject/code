using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using sdLitica.Events.Abstractions;
using sdLitica.Utils.Models;

namespace sdLitica.Events.Integration
{
    public class AnalyticModuleRegistrationRequestEvent : Event
    {
        public AnalyticModuleRegistrationRequestEvent() : base()
        {

        }
        public AnalyticModuleRegistrationRequestEvent(AnalyticsModuleRegistrationModel module) : base()
        {
            Module = module;
        }
        [JsonProperty]
        public AnalyticsModuleRegistrationModel Module { get; set; }
    }
}
