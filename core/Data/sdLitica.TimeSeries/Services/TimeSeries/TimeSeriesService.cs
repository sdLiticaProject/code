using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDB.Client.Core.Flux.Domain;
using Newtonsoft.Json.Linq;

namespace sdLitica.TimeSeries.Services
{
    public class TimeSeriesService: ITimeSeriesService
    {
        private TimeSeriesSettings TimeSeriesSettings { set; get; }

        private readonly InfluxDBClient _influxClient;
        private readonly ITimeSeriesMetadataService _timeSeriesMetadataService;
        private readonly IAppSettings _settings;

        public TimeSeriesService(IAppSettings settings, ITimeSeriesMetadataService timeSeriesMetadataService)
        {
            TimeSeriesSettings = settings.TimeSeriesSettings;
            // _influxClient = InfluxDBClientFactory.Create(TimeSeriesSettings.InfluxHostName,TimeSeriesSettings.InfluxToken);
            _influxClient = InfluxDBClientFactory.Create(InfluxDBClientOptions.Builder.CreateNew()
                .Url(TimeSeriesSettings.InfluxHostName)
                .AuthenticateToken(TimeSeriesSettings.InfluxToken)
                .TimeOut(TimeSpan.FromSeconds(10000))
                .Build()
            );
            _timeSeriesMetadataService = timeSeriesMetadataService;
            _settings = settings;
        }


        public async Task<User> CreateUser(string username, string password)
        {
            var result = await _influxClient.GetUsersApi().CreateUserAsync(username);
            await _influxClient.GetUsersApi().UpdateUserPasswordAsync(result.Id,"",password);
            return result;
        }

        public async Task<string> AddRandomTimeSeries(string bucketId)
        {
            string measurementName = Guid.NewGuid().ToString();
            PointData[] points = CreatePointsDataStartingAt(DateTime.UtcNow.AddMonths(-11), 500,
                measurementName);
            await _influxClient.GetWriteApiAsync().WritePointsAsync(points,bucketId,TimeSeriesSettings.InfluxOrgId);
            return measurementName;
        }

        public async Task<string> AddRandomTimeSeries(string measurementId, string bucketId)
        {
            PointData[] points = CreatePointsDataStartingAt(DateTime.UtcNow.AddMonths(-11), 500,
                measurementId);
            await _influxClient.GetWriteApiAsync().WritePointsAsync(points, bucketId, TimeSeriesSettings.InfluxOrgId);
            
            return measurementId;
        }

        public async Task<List<FluxTable>> ReadMeasurementById(string measurementId, string bucketId)
        {
            var flux = $"from(bucket:\"{bucketId}\")"
                       + " |> range(start: 0)"
                       + $" |> filter(fn: (r) => r._measurement == \"{measurementId}\")";

            var queryApi = _influxClient.GetQueryApi();

            //
            // QueryData
            //
            var tables = await queryApi.QueryAsync(flux,TimeSeriesSettings.InfluxOrgId);

            return tables;
        }

        public async Task<List<FluxTable>> ReadMeasurementById(string measurementId, string bucketId, string from, string to, string step, string agrFn)
        {
            StringBuilder query = new StringBuilder($"from(bucket:\"{bucketId}\")");

            bool hasStartTime = !string.IsNullOrWhiteSpace(from);
            bool hasEndTime = !string.IsNullOrWhiteSpace(to);
            if (hasStartTime)
            {
                query.Append($" |> range(start: {from}");
            }
            else
            {
                query.Append($" |> range(start: 0");
            }

            if (hasEndTime)
            {
                query.Append($", stop: {to})");
            }
            else
            {
                query.Append(")");
            }

            query.Append($" |> filter(fn: (r) => r._measurement == \"{measurementId}\")");

            if (!string.IsNullOrWhiteSpace(step))
            {
                query.Append($"|> aggregateWindow(every: {step}, fn: {(!string.IsNullOrWhiteSpace(agrFn) ? agrFn : "mean")}, createEmpty: false) " +
                             // $"|> yield(name: \"{(!string.IsNullOrWhiteSpace(agrFn) ? agrFn : "mean")}\") " +
                             $"|> sort(columns: [\"_time\"])");
            }

            var queryApi = _influxClient.GetQueryApi();

            //
            // QueryData
            //
            var tables = await queryApi.QueryAsync(query.ToString(),TimeSeriesSettings.InfluxOrgId);

            return tables;
        }

        public async Task<string> CreateFileCSV(string measurementId, string bucketId)
        {
            StringBuilder query = new StringBuilder($"from(bucket:\"{bucketId}\")");
            query.Append($" |> range(start: 0)");

            query.Append($" |> filter(fn: (r) => r._measurement == \"{measurementId}\")");
            
            query.Append("|> pivot(rowKey:[\"_time\"], columnKey:[\"_field\"], valueColumn:\"_value\")");
            query.Append("|> drop(columns:[\"_start\", \"_stop\", \"_measurement\"])");

            var queryApi = _influxClient.GetQueryApi();

            var csv = await queryApi.QueryRawAsync(query.ToString(), org: TimeSeriesSettings.InfluxOrgId);

            return csv;

        }

        public async Task DeleteMeasurementById(string measurementId, string bucketId)
        {
            await _influxClient.GetDeleteApi().Delete(
                new DeletePredicateRequest(
                    predicate: $"_measurement=\"{measurementId}\"",
                    start: new DateTime(1900,11,14), 
                    stop: DateTime.Now
                    ), 
                bucketId,TimeSeriesSettings.InfluxOrgId
                );
        }

        private PointData[] CreatePointsDataStartingAt(DateTime start, int rows, string measurementName)
        {
            Random rng = new Random();
            string[] regions = new[] {"west-eu", "north-eu", "west-us", "east-us", "asia"};
            string[] hosts = new[] {"some-host", "some-other-host"};

            DateTime timestamp = start;
            PointData[] infos = new PointData[rows];
            for (int i = 0; i < rows; i++)
            {
                long ram = rng.Next(int.MaxValue);
                double cpu = rng.NextDouble();
                string region = regions[rng.Next(regions.Length)];
                string host = hosts[rng.Next(hosts.Length)];

                var info = PointData.Measurement(measurementName)
                    .Tag("host", host)
                    .Tag("region", region)
                    .Field("cpu", cpu)
                    .Field("ram", ram)
                    .Timestamp(timestamp, WritePrecision.Ns);
                infos[i] = info;

                timestamp = timestamp.AddSeconds(1);
            }

            return infos;
        }

        /// <summary>
        /// Uploads data from csv file to measurement.
        /// 1st column is timestamp.
        /// Columns starting with '_' ('_' will be deleted on InfluxDB) are tags, and others are fields.
        /// </summary>
        /// <param name="measurementId">MeasurementId</param>
        /// <param name="bucketId"></param>
        /// <param name="lines">Lines from csv file.</param>
        /// <returns></returns>
        public async
            Task<(int columnsCount, int rowCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)>
            UploadDataFromCsv(string measurementId, string bucketId, List<string> lines)
        {
            await DeleteMeasurementById(measurementId, bucketId);

            string[] headers = lines[0].Split(',');
            PointData[] influxRows = new PointData[lines.Count - 1];
            
            var columnsCount = headers.Length - 1;
            var rowCount = lines.Count - 1;
            var columns = new HashSet<string>();
            var tags = new Dictionary<string, HashSet<string>>();

            foreach (var h in headers)
            {
                if (h.StartsWith("_")) {
                    tags[h[1..]] =  new HashSet<string>();
                }
                else if (!h.Contains("time"))
                {
                    columns.Add(h);
                }
            }

            for (int i = 1; i < lines.Count; i++)
            {
                string[] rowValues = lines[i].Split(',');

                PointData row = PointData.Measurement(measurementId)
                    .Timestamp(DateTime.FromFileTimeUtc(DateTime.Parse(rowValues[0]).ToFileTimeUtc()),WritePrecision.Ns);

                for (int j = 1; j < headers.Length; j++)
                {
                    if (headers[j].StartsWith('_'))
                    {
                        row = row.Tag(headers[j].Substring(1), rowValues[j]);
                        tags[headers[j][1..]].Add(rowValues[j]);
                    }
                    else
                    {
                        row = row.Field(headers[j], Double.Parse(rowValues[j], CultureInfo.InvariantCulture));
                    }
                }

                influxRows[i - 1] = row;
            }
            await _influxClient.GetWriteApiAsync().WritePointsAsync(influxRows, bucketId, TimeSeriesSettings.InfluxOrgId);
            return (columnsCount, rowCount, columns, tags);
        }
        
        public async Task<(int columnsCount, int rowCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)> UploadDataFromJTs(string measurementId, string bucketId, TimeSeriesJTSEntity jsonTsData)
        {
            await DeleteMeasurementById(measurementId, bucketId);
            
            var columnsCount = jsonTsData.Header.Columns.Count;
            var rowCount = jsonTsData.Header.RecordCount;
            var columns = new HashSet<string>();
            var tags = new Dictionary<string, HashSet<string>>();

            List<PointData> influxRows = new List<PointData>();
        
            foreach (var item in jsonTsData.Data)
            {
                PointData row = PointData.Measurement(measurementId)
                    .Timestamp(DateTime.FromFileTimeUtc(DateTime.Parse(item.Ts.ToString()).ToFileTimeUtc()), WritePrecision.Ns);
        
                foreach (var column in jsonTsData.Header.Columns)
                {
                    string columnName = column.Key;
                    var columnData = item.F[columnName];

                    if (column.Value.Name.StartsWith("_") && !tags.ContainsKey(column.Value.Name))
                    {
                        tags[column.Value.Name[1..]] = new HashSet<string>();
                    }
                    else
                    {
                        columns.Add(column.Value.Name);
                    }

                    if (columnData != null)
                    {
                        double value;
                        bool success = Double.TryParse((ReadOnlySpan<char>)columnData.V.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
                        if (success)
                        {
                            if (column.Value.Name.StartsWith("_"))
                            {
                                row = row.Tag(column.Value.Name, value.ToString());
                                tags[column.Value.Name[1..]].Add(value.ToString());
                            } else {
                                if (column.Value.DataType == "NUMBER")
                                {
                                    row = row.Field(column.Value.Name, value);
                                }
                                else if (column.Value.DataType == "TEXT")
                                {
                                    row = row.Field(column.Value.Name, value.ToString());
                                }
                            }
                        }
                    }
                }
                influxRows.Add(row);
            }
            
            await _influxClient.GetWriteApiAsync().WritePointsAsync(influxRows.ToArray(), bucketId, TimeSeriesSettings.InfluxOrgId);
            return (columnsCount, rowCount, columns, tags);
        }

        public async
            Task<(int columnsCount, int rowCount, HashSet<string> columns, Dictionary<string, HashSet<string>> tags)>
            UploadDataFromWebSocket(string timeSeriesId, string bucketId, TimeSeriesWebSocketEntity jsonTsData)
        {
                var tm = _timeSeriesMetadataService.GetTimeSeriesMetadata(timeSeriesId);
                
                var columnsCount = jsonTsData.Fields.Count + jsonTsData.Tags.Count;
                var rowCount = tm.RowsCount;


                HashSet<string> columns = JsonSerializer.Deserialize<HashSet<string>>(tm.Columns);
                Dictionary<string, HashSet<string>> tags =
                    JsonSerializer.Deserialize<Dictionary<string, HashSet<string>>>(tm.Tags);
                

                List<PointData> influxRows = new List<PointData>();

                for (int i = 0; i < jsonTsData.Time.Count; i++)
                {
                    PointData row = PointData.Measurement(tm.InfluxId.ToString()).Timestamp(DateTime.FromFileTimeUtc(DateTime.Parse(jsonTsData.Time[i]).ToFileTimeUtc()), WritePrecision.Ns);

                    rowCount += 1;

                    foreach (var tag in jsonTsData.Tags)
                    {
                        if (!tags.ContainsKey(tag.Key[1..]))
                        {
                            tags[tag.Key[1..]] = new HashSet<string>();
                        }

                        row = row.Tag(tag.Key[1..], tag.Value[i]);
                        tags[tag.Key[1..]].Add(tag.Value[i]);
                    }

                    foreach (var field in jsonTsData.Fields)
                    {
                        columns.Add(field.Key);
                        row = row.Field(field.Key, Double.Parse(field.Value[i]));
                    }

                    influxRows.Add(row);
                }

                await _influxClient.GetWriteApiAsync()
                    .WritePointsAsync(influxRows.ToArray(), bucketId, TimeSeriesSettings.InfluxOrgId);
                
                return (columnsCount, rowCount, columns, tags);
        }
    }
}