using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;
using sdLitica.Utils.Settings;
using sdLitica.Utils.Abstractions;

namespace sdLitica.TimeSeries.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private TimeSeriesSettings TimeSeriesSettings { set; get; }

        private readonly InfluxClient _influxClient;
        private readonly IAppSettings _settings;

        public TimeSeriesService(IAppSettings settings)
        {
            TimeSeriesSettings = settings.TimeSeriesSettings;
            _influxClient = new InfluxClient(new Uri(TimeSeriesSettings.InfluxHostName));
            _settings = settings;
        }


        public async Task<InfluxResult> CreateUser(string username, string password)
        {
            var result = await _influxClient.CreateUserAsync(username, password);
            return result;
        }

        public async Task<string> AddRandomTimeSeries()
        {
            var rows = CreateDynamicRowsStartingAt(new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc), 500,
                out var measurementName);
            await _influxClient.WriteAsync(TimeSeriesSettings.InfluxDatabase, rows);
            return measurementName;
        }

        public async Task<InfluxResult<DynamicInfluxRow>> ReadMeasurementById(string measurementId)
        {
            var resultSet = await _influxClient.ReadAsync<DynamicInfluxRow>(TimeSeriesSettings.InfluxDatabase,
                "SELECT * FROM " + "\"" + measurementId + "\"");

            // resultSet will contain 1 result in the Results collection (or multiple if you execute multiple queries at once)
            var result = resultSet.Results[0];

            return result;
        }

        public async Task<InfluxResult> DeleteMeasurementById(string measurementId)
        {
            var result = await _influxClient.DropSeries(TimeSeriesSettings.InfluxDatabase, measurementId);
            return result;
        }

        private NamedDynamicInfluxRow[] CreateDynamicRowsStartingAt(DateTime start, int rows,
            out string measurementName)
        {
            var rng = new Random();
            var regions = new[] {"west-eu", "north-eu", "west-us", "east-us", "asia"};
            var hosts = new[] {"some-host", "some-other-host"};

            var timestamp = start;
            var infos = new NamedDynamicInfluxRow[rows];
            measurementName = Guid.NewGuid().ToString();
            for (var i = 0; i < rows; i++)
            {
                long ram = rng.Next(int.MaxValue);
                double cpu = rng.NextDouble();
                string region = regions[rng.Next(regions.Length)];
                string host = hosts[rng.Next(hosts.Length)];

                var info = new NamedDynamicInfluxRow();
                info.Fields.Add("cpu", cpu);
                info.Fields.Add("ram", ram);
                info.Tags.Add("host", host);
                info.Tags.Add("region", region);
                info.Timestamp = timestamp;
                info.MeasurementName = measurementName;
                infos[i] = info;

                timestamp = timestamp.AddSeconds(1);
            }

            return infos;
        }

        public async Task<List<MeasurementRow>> ReadAllMeasurements()
        {
            var resultSet = await _influxClient.ShowMeasurementsAsync(TimeSeriesSettings.InfluxDatabase);
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