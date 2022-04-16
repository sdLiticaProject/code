using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using sdLitica.Exceptions.Http;
using sdLitica.Triggers.Jobs;

namespace sdLitica.Triggers.Services
{
	public class TriggerService : ITriggersService
	{
		private readonly ILogger _logger;
		private readonly ISchedulerFactory _schedulerFactory;
		private IScheduler Scheduler => _schedulerFactory.GetScheduler().Result;
		private readonly IServiceProvider _provider;

		public TriggerService(ILoggerFactory logger, IServiceProvider provider, ISchedulerFactory schedulerFactory)
		{
			_provider = provider;
			_schedulerFactory = schedulerFactory;
			// _scheduler = new StdSchedulerFactory().GetScheduler().Result;
			// _scheduler.JobFactory = new DiJobFactory(provider);
			// _scheduler.Start();

			_logger = logger.CreateLogger(nameof(TriggerService));
		}

		public void AddNewTrigger(Guid metadataId, string cronSchedule, string fetchUrl)
		{
			_logger.LogDebug("Adding new trigger. Id: {MetadataId}, cron: {CronSchedule}",
				metadataId,
				cronSchedule);

			if (!CronExpression.IsValidExpression(cronSchedule))
			{
				_logger.LogWarning(
					"Could not create trigger {Trigger} with cron schedule {CronSchedule}",
					metadataId, cronSchedule);
				throw new InvalidRequestException(
					$"Could not create trigger {metadataId} with cron schedule {cronSchedule}");
			}

			if (Scheduler.GetJobDetail(new JobKey(metadataId.ToString())).Result != null)
			{
				throw new InvalidRequestException($"Trigger with id {metadataId} already exists");
			}

			var job = JobBuilder.Create<AppendDataJob>()
				.WithIdentity(metadataId.ToString())
				.UsingJobData("metadataId", metadataId.ToString())
				.UsingJobData("fetchUrl", fetchUrl)
				.Build();

			try
			{
				var trigger = TriggerBuilder.Create().WithIdentity(metadataId.ToString())
					.StartNow()
					.WithCronSchedule(cronSchedule)
					.Build();

				Scheduler.ScheduleJob(job, trigger);
			}
			catch (Exception e)
			{
				_logger.LogWarning(
					"Could not create trigger {Trigger} with cron schedule {CronSchedule}. Exception occured: {Exception}",
					metadataId, cronSchedule, e);
				throw;
			}
		}

		public async Task EditTrigger(Guid metadataId, string cronSchedule, string fetchUrl)
		{
			try
			{
				_logger.LogDebug("Editing trigger. Id: {MetadataId}, cron: {CronSchedule}",
					metadataId,
					cronSchedule);

				if (!CronExpression.IsValidExpression(cronSchedule))
				{
					_logger.LogWarning(
						"Could not edit trigger {Trigger} with cron schedule {CronSchedule}",
						metadataId, cronSchedule);
					throw new InvalidRequestException(
						$"Could not edit trigger {metadataId} with cron schedule {cronSchedule}");
				}

				var trigger = TriggerBuilder.Create()
					.WithIdentity(metadataId.ToString())
					.ForJob(metadataId.ToString())
					.UsingJobData("fetchUrl", fetchUrl)
					.StartNow()
					.WithCronSchedule(cronSchedule)
					.Build();

				await Scheduler.RescheduleJob(new TriggerKey(metadataId.ToString()), trigger);
			}
			catch (Exception e)
			{
				_logger.LogWarning(
					"Could not reschedule trigger with cron schedule {CronSchedule}. Exception occured: {Exception}",
					cronSchedule, e);
				throw;
			}
		}

		public void RemoveTrigger(Guid metadataId)
		{
			var success = Scheduler.DeleteJob(new JobKey(metadataId.ToString())).Result;
			if (!success)
			{
				throw new NotFoundException($"Could not find job with id {metadataId}");
			}
		}

		public void PauseJob(Guid metadataId)
		{
			Scheduler.PauseJob(new JobKey(metadataId.ToString()));
		}

		public void ResumeJob(Guid metadataId)
		{
			Scheduler.ResumeJob(new JobKey(metadataId.ToString()));
		}

		public IJobDetail? GetJobInfo(Guid metadataId)
		{
			return Scheduler.GetJobDetail(new JobKey(metadataId.ToString())).Result;
		}

		public ITrigger? GetTrigger(Guid metadataId)
		{
			return Scheduler.GetTrigger(new TriggerKey(metadataId.ToString())).Result;
		}
	}
}
