using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using sdLitica.Bootstrap.Settings;

namespace sdLitica.Bootstrap.Extensions
{
    /// <summary>
    /// Extensions class for the IConfiguration class
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// This method add settings to be used in the bootstrap of the application
        /// </summary>
        /// <param name="configuration"></param>
        public static void AddSettings(this IConfiguration configuration)
        {            
            BootstrapSettings.AppSettings = 
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
