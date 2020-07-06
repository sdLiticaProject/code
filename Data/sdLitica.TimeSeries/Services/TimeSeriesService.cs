using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;
using sdLitica.Utils.Settings;
using sdLitica.Utils.Abstractions;
using System.IO;
using System.Text;

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
            string measurementName = Guid.NewGuid().ToString();
            var rows = CreateDynamicRowsStartingAt(new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc), 500,
                measurementName);
            await _influxClient.WriteAsync(TimeSeriesSettings.InfluxDatabase, rows);
            return measurementName;
        }

        public async Task<string> AddRandomTimeSeries(string measurementId)
        {
            NamedDynamicInfluxRow[] rows = CreateDynamicRowsStartingAt(new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc), 500,
                measurementId);
            await _influxClient.WriteAsync(TimeSeriesSettings.InfluxDatabase, rows);
            return measurementId;
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
            string measurementName)
        {
            var rng = new Random();
            var regions = new[] {"west-eu", "north-eu", "west-us", "east-us", "asia"};
            var hosts = new[] {"some-host", "some-other-host"};

            var timestamp = start;
            var infos = new NamedDynamicInfluxRow[rows];
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

        public async Task<string> UploadDataFromCsv(string measurementId, List<string> lines)
        {
            await DeleteMeasurementById(measurementId);
            string[] headers = lines[0].Split(',');
            NamedDynamicInfluxRow[] influxRows = new NamedDynamicInfluxRow[lines.Count - 1];
            for (int i = 1; i < lines.Count; i++)
            {
                string[] rowValues = lines[i].Split(',');

                NamedDynamicInfluxRow row = new NamedDynamicInfluxRow();
                for (int j = 1; j < headers.Length; j++)
                {
                    row.Fields.Add(headers[j], rowValues[j]);
                }
                row.Timestamp = DateTime.Parse(rowValues[0]);
                row.MeasurementName = measurementId;
                influxRows[i - 1] = row;
            }

            await _influxClient.WriteAsync(TimeSeriesSettings.InfluxDatabase, influxRows);
            return "?";
        }

    }
}