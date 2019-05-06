using Microsoft.Extensions.Configuration;
using sdLitica.Utils.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Utils.Settings
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration _configuration;
        private readonly string _securitySection = "Security";
        
        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public int TokenExpirationInHours =>
            int.Parse(_configuration.GetSection(_securitySection)["TokenExpirationInHours"]);
    }
}
