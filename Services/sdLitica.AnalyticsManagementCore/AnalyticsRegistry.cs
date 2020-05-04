using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.AnalyticsManagementCore
{
    public class AnalyticsRegistry
    {
        private readonly IDictionary<string, AnalyticsOperation> _analyticsRegistry;
        private readonly IDictionary<Guid, DateTime> _moduleLastHeardTime;
        public AnalyticsRegistry()
        {
            _analyticsRegistry = new Dictionary<string, AnalyticsOperation>();
            _moduleLastHeardTime = new Dictionary<Guid, DateTime>();
        }


        public void Register()
        {

        }

        public string GetQueue()
        {
            throw new NotImplementedException();
        }
    }
}
