using Microsoft.Extensions.Configuration;
using sdLitica.Bootstrap.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Bootstrap.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddSettings(this IConfiguration configuration)
        {            
            BootstrapSettings.AppSettings = 
                configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
    }
}
