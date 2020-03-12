using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Bus
{
    public static class Exchanges
    {
        public static readonly string TimeSeries = "TimeSeriesQueue"; 

        public static IEnumerable<string> GetExchanges()
        {
            return new List<string>()
            {
                TimeSeries
            };
        }
    }
}
