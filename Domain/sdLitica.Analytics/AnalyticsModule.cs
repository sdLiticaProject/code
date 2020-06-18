using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics
{
    public class AnalyticsModule : Entity
    {
        public AnalyticsModule()
        {

        }
        public Guid Id { get; set; }
        public DateTime LastHeardTime { get; set; }
        public string QueueName { get; set; } // todo
    }
}
