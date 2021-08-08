using Newtonsoft.Json;
using sdLitica.Events.Abstractions;

namespace sdLitica.Events.Integration
{
    public class ModuleHeartbeatEvent: Event
    {
        [JsonProperty]
        public string ModuleName { get; set; }

        public ModuleHeartbeatEvent(string moduleName)
        {
            ModuleName = moduleName;
        }
    }
}