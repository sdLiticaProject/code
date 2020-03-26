﻿using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Bus
{
    /// <summary>
    /// Exchanges available in the systemas a whole
    /// </summary>
    public static class Exchanges
    {
        /// <summary>
        /// Time Series Exchange
        /// </summary>
        public static readonly string TimeSeries = "example.request.queue";//"TimeSeriesQueue";
        //public static readonly string Analytics = "AnalyticsQueue";

        /// <summary>
        /// Get Exchanges
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> GetExchanges()
        {
            return new List<string>()
            {
                TimeSeries
            };
        }
    }
}
