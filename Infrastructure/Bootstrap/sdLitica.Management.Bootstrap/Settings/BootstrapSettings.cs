using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace sdLitica.Bootstrap.Settings
{
    internal static class BootstrapSettings
    {
        internal static IConfiguration AppSettings { get; set; }
    }
}
