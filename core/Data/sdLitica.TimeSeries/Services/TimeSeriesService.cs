using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.TimeSeries.Services
{
    public class TimeSeriesService: ITimeSeriesService
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
            InfluxResult result = await _influxClient.CreateUserAsync(username, password);
            return result;
        }

        public async Task<string> AddRandomTimeSeries()
        {
            string measurementName = Guid.NewGuid().ToString();
            NamedDynamicInfluxRow[] rows = CreateDynamicRowsStartingAt(new DateTime(2010, 1, 1, 1, 1, 1, DateTimeKind.Utc), 500,
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
            InfluxResultSet<DynamicInfluxRow> resultSet = await _influxClient.ReadAsync<DynamicInfluxRow>(TimeSeriesSettings.InfluxDatabase,
                "SELECT * FROM " + "\"" + measurementId + "\"");

            // resultSet will contain 1 result in the Results collection (or multiple if you execute multiple queries at once)
            InfluxResult<DynamicInfluxRow> result = resultSet.Results[0];

            return result;
        }

        public async Task<InfluxResult<DynamicInfluxRow>> ReadMeasurementById(string measurementId, string from, string to, string step)
        {
            StringBuilder query = new StringBuilder($"SELECT last(*) FROM \"{measurementId}\"");
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            bool hasStartTime = !string.IsNullOrWhiteSpace(from);
            bool hasEndTime = !string.IsNullOrWhiteSpace(to);
            if (hasStartTime || hasEndTime)
            {
                query.Append(" WHERE ");
                if (hasStartTime)
                {
                    query.Append("time >= $from");
                    parameters.Add("from", from);
                }
                if (hasStartTime && hasEndTime) query.Append(" AND ");
                if (hasEndTime)
                {
                    query.Append("time <= $to");
                    parameters.Add("to", to);
                }
            }

            // Vibrant.InfluxDB.Client substitutes all parameters with single quotes (e.g. ...GROUP BY time('1m'))
            // but InfluxDB requires parameter for time(<interval>) function without quotes (e.g. ...time(1m))
            // so, the <step> string parameter should be validated in controller when received from user
            if (!string.IsNullOrWhiteSpace(step)) query.Append($" GROUP BY time({step})");

            InfluxResultSet<DynamicInfluxRow> resultSet = await _influxClient.ReadAsync<DynamicInfluxRow>(TimeSeriesSettings.InfluxDatabase, query.ToString(), parameters);

            // resultSet will contain 1 result in the Results collection (or multiple if you execute multiple queries at once)
            InfluxResult<DynamicInfluxRow> result = resultSet.Results[0];

            return result;
        }

        public async Task<InfluxResult> DeleteMeasurementById(string measurementId)
        {
            InfluxResult result = await _influxClient.DropSeries(TimeSeriesSettings.InfluxDatabase, measurementId);
            return result;
        }

        private NamedDynamicInfluxRow[] CreateDynamicRowsStartingAt(DateTime start, int rows, string measurementName)
        {
            Random rng = new Random();
            string[] regions = new[] {"west-eu", "north-eu", "west-us", "east-us", "asia"};
            string[] hosts = new[] {"some-host", "some-other-host"};

            DateTime timestamp = start;
            NamedDynamicInfluxRow[] infos = new NamedDynamicInfluxRow[rows];
            for (int i = 0; i < rows; i++)
            {
                long ram = rng.Next(int.MaxValue);
                double cpu = rng.NextDouble();
                string region = regions[rng.Next(regions.Length)];
                string host = hosts[rng.Next(hosts.Length)];

                NamedDynamicInfluxRow info = new NamedDynamicInfluxRow();
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
            InfluxResult<MeasurementRow> resultSet = await _influxClient.ShowMeasurementsAsync(TimeSeriesSettings.InfluxDatabase);
            if (resultSet.Series.Count > 0)
            {
                InfluxSeries<MeasurementRow> result = resultSet.Series[0];
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