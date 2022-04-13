using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using sdLitica.TimeSeries.Services;
using sdLitica.Triggers.Services;

namespace sdLitica.Triggers.Jobs
{
	public class AppendDataJob : IJob
	{
		private ILogger Logger { get; }
		private ITimeSeriesMetadataService SeriesMetadataService { get; }
		private ITimeSeriesService TimeSeriesService { get; }

		public AppendDataJob(ILogger logger,
			ITimeSeriesMetadataService seriesMetadataService,
			ITimeSeriesService timeSeriesService)
		{
			Logger = logger;
			SeriesMetadataService = seriesMetadataService;
			TimeSeriesService = timeSeriesService;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				Logger.LogDebug("Stub job execution started");
				var metaId = context.MergedJobDataMap.GetString("metadataId");
				Logger.LogDebug("Id of metadata is {MetadataId}", metaId);
				Logger.LogDebug("InfluxId is {InfluxId}",
					SeriesMetadataService.GetTimeSeriesMetadata(metaId).InfluxId);
				// some job execution
				Logger.LogDebug("Stub job execution ended");
			}
			catch (Exception e)
			{
				Logger.LogWarning("Job failed with exception {Exception}", e);
			}
		}

	}
}
