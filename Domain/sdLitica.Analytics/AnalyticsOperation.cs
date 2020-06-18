using sdLitica.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Analytics
{
    public class AnalyticsOperation : Entity
    {
        public AnalyticsOperation()
        {

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AnalyticsModule Module { get; set; }
        public string Description { get; set; }
    }
}
