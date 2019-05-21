using Microsoft.Extensions.Configuration;
using sdLitica.Utils.Abstractions;

namespace sdLitica.Utils.Settings
{
    public class AppSettings : IAppSettings
    {
        private readonly IConfiguration _configuration;
        private readonly string _securitySection = "Security";
        private readonly string _timeSeriesSection = "TimeSeries";
        private readonly TimeSeriesSettings _timeSeriesSettings;

        public AppSettings(IConfiguration configuration)
        {
            _configuration = configuration;
            _timeSeriesSettings = new TimeSeriesSettings();
        }

        public int TokenExpirationInHours =>
            int.Parse(_configuration.GetSection(_securitySection)["TokenExpirationInHours"]);

        public TimeSeriesSettings TimeSeriesSettings            
        {
            get
            {
                _configuration.GetSection(_timeSeriesSection).Bind(_timeSeriesSettings);
                return _timeSeriesSettings;
            }            
        }
    }
}
