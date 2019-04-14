using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using sdLitica.Helpers;
using sdLitica.WebAPI.Services;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.WebAPI.Services
{
    public class InfluxDbService : IInfluxDB
    {
        private AppSettings AppSettings { set; get; }

        private InfluxClient _influxClient;

        public InfluxDbService(IOptions<AppSettings> settings)
        {
            AppSettings = settings.Value;
            _influxClient = new InfluxClient(new Uri(AppSettings.InfluxHostName));
        }

        public async Task<InfluxResult> CreateUser(string username, string password)
        {
            var result = await _influxClient.CreateUserAsync(username, password);
            return result;
        }

        public async Task<List<MeasurementRow>> ReadAllMeasurements()
        {
            var resultSet = await _influxClient.ShowMeasurementsAsync(AppSettings.InfluxDatabase);
            if (resultSet.Series.Count > 0)
            {
                var result = resultSet.Series[0];
                return result.Rows;
            }
            else
            {
                return new List<MeasurementRow>();
            }
        }
    }
}