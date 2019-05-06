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
    }
}
