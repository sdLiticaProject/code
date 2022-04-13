using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using sdLitica.TimeSeries.Services;

namespace sdLitica.Triggers.Jobs
{
	public class AppendDataJob : IJob
	{
		private ILogger Logger { get; }
		private ITimeSeriesMetadataService SeriesMetadataService { get; }
		private ITimeSeriesService TimeSeriesService { get; }

		public AppendDataJob(ILoggerFactory logger,
			ITimeSeriesMetadataService seriesMetadataService,
			ITimeSeriesService timeSeriesService)
		{
			Logger = logger.CreateLogger(nameof(AppendDataJob));
			SeriesMetadataService = seriesMetadataService;
			TimeSeriesService = timeSeriesService;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				Logger.LogDebug("Append job execution started");
				var metaId = context.MergedJobDataMap.GetString("metadataId");
				var url = context.MergedJobDataMap.GetString("fetchUrl");
				Logger.LogDebug("Id of metadata is {MetadataId}", metaId);
				var metadata = SeriesMetadataService.GetTimeSeriesMetadata(metaId);
				Logger.LogDebug("InfluxId is {InfluxId}", metadata.InfluxId);
				await TimeSeriesService.AppendDataFromJson(metaId, FetchJsonData(url!), metadata.Columns);
				// some job execution
				Logger.LogDebug("Stub job execution ended");
			}
			catch (Exception e)
			{
				Logger.LogWarning("Job failed with exception {Exception}", e);
			}
		}

		private JArray FetchJsonData(string url)
		{
			var client = new HttpClient();
			var data = client.GetStringAsync(url).Result;
			Logger.LogDebug("Gor from {Url} response {Data}", url, data);
			return JArray.Parse(data);
		}
	}
}
