using sdLitica.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
