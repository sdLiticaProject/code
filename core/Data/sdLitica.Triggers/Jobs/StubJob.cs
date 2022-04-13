using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using sdLitica.Triggers.Services;

namespace sdLitica.Triggers.Jobs
{
	public class StubJob : IJob
	{
		private ILogger Logger { get; }


		public StubJob(ILogger logger)
		{
			Logger = logger;
		}

		public async Task Execute(IJobExecutionContext context)
		{
			try
			{
				Logger.LogDebug("Stub job execution started");
				var metaId = context.MergedJobDataMap.GetString("metadataId");
				Logger.LogDebug("Id of metadata is {MetadataId}", metaId);
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
