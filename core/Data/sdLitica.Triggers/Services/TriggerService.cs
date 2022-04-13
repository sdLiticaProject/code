using System;
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
		private readonly IScheduler _scheduler;
		private readonly IServiceProvider _provider;

		public TriggerService(ILoggerFactory logger, IServiceProvider provider)
		{
			_provider = provider;
			_scheduler = new StdSchedulerFactory().GetScheduler().Result;
			// _scheduler.JobFactory = new DiJobFactory(provider);
			_scheduler.Start();

			_logger = logger.CreateLogger(nameof(TriggerService));
		}

		public void AddNewTrigger(Guid metadataId, string cronSchedule)
		{
			_logger.LogDebug("Adding new trigger. Id: {MetadataId}, cron: {CronSchedule}",
				metadataId,
				cronSchedule);

			if (_scheduler.GetJobDetail(new JobKey(metadataId.ToString())).Result != null)
			{
				throw new InvalidRequestException($"Trigger with id {metadataId} already exists");
			}

			var job = JobBuilder.Create<AppendDataJob>().WithIdentity(metadataId.ToString()).UsingJobData("metadataId", metadataId.ToString()).Build();

			try
			{
				var trigger = TriggerBuilder.Create().WithIdentity(metadataId.ToString())
					.StartNow()
					.WithCronSchedule(cronSchedule)
					.Build();

				_scheduler.ScheduleJob(job, trigger);
			}
			catch (SchedulerException e)
			{
				_logger.LogWarning(
					"Could not create trigger {Trigger} with cron schedule {CronSchedule}. Exception occured: {Exception}",
					metadataId, cronSchedule, e);
				throw new InvalidRequestException(
					$"Could not create trigger {metadataId} with cron schedule {cronSchedule}. Exception occured: {e}");
			}
		}

		public void EditTrigger(Guid metadataId, string cronSchedule)
		{
			try
			{
				var trigger = TriggerBuilder.Create().WithIdentity(metadataId.ToString())
					.ForJob(metadataId.ToString())
					.StartNow()
					.WithCronSchedule(cronSchedule)
					.Build();
				_scheduler.RescheduleJob(new TriggerKey(metadataId.ToString()), trigger);
			}
			catch (SchedulerException e)
			{
				_logger.LogWarning(
					"Could not reschedule trigger with cron schedule {CronSchedule}. Exception occured: {Exception}",
					cronSchedule, e);
				throw new ArgumentException(
					$"Could not reschedule trigger with cron schedule {cronSchedule}. Exception occured: {e}");
			}
		}

		public void RemoveTrigger(Guid metadataId)
		{
			var success = _scheduler.DeleteJob(new JobKey(metadataId.ToString())).Result;
			if (!success)
			{
				throw new NotFoundException($"Could not find job with id {metadataId}");
			}
		}

		public void PauseJob(Guid metadataId)
		{
			_scheduler.PauseJob(new JobKey(metadataId.ToString()));
		}

		public void ResumeJob(Guid metadataId)
		{
			_scheduler.ResumeJob(new JobKey(metadataId.ToString()));
		}

		public IJobDetail? GetJobInfo(Guid metadataId)
		{
			return _scheduler.GetJobDetail(new JobKey(metadataId.ToString())).Result;
		}

		public ITrigger? GetTrigger(Guid metadataId)
		{
			return _scheduler.GetTrigger(new TriggerKey(metadataId.ToString())).Result;
		}
	}
}
