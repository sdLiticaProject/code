using System;
using System.Collections.Generic;
using System.Text;
using sdLitica.Utils.Settings;

namespace sdLitica.Utils.Abstractions
{
    /// <summary>
    /// This interface provides access to configurations inside `appsettings.json`
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Provides Token Expiration in Hours
        /// </summary>
        int TokenExpirationInHours { get; }

        /// <summary>
        /// Provides setting for TimeSeries (i.e. InfluxDB)
        /// </summary>
        TimeSeriesSettings TimeSeriesSettings { get; }

        /// <summary>
        /// Provides setting for Messages (i.e. RabbitMQ)
        /// </summary>
        MessageSettings MessageSettings { get; }

        /// <summary>
        /// Provides setting for Analytics (e.g. timeouts for analytical modules)
        /// </summary>
        AnalyticsSettings AnalyticsSettings { get; }
    }
}
