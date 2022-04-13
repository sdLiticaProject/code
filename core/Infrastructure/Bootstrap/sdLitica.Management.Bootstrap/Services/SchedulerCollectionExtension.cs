using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using sdLitica.TimeSeries.Services;
using sdLitica.Triggers.Jobs;
using sdLitica.Triggers.Services;

namespace sdLitica.Bootstrap.Services
{
	internal static class SchedulerCollectionExtensions
	{
		internal static void AddSchedulingServices(this IServiceCollection services)
		{
			services.AddScoped<ITriggersService, TriggerService>();
			services.AddTransient<StubJob>();
			services.AddTransient<AppendDataJob>();

			// if you are using persistent job store, you might want to alter some options
			services.Configure<QuartzOptions>(options =>
			{
				options.Scheduling.IgnoreDuplicates = true; // default: false
				options.Scheduling.OverWriteExistingData = true; // default: true
			});

			services.AddQuartz(q =>
			{
				q.UseMicrosoftDependencyInjectionJobFactory();
				q.UseSimpleTypeLoader();
				q.UseInMemoryStore();
				q.UseDefaultThreadPool(tp =>
				{
					tp.MaxConcurrency = 5;
				});
			});
			services.AddQuartzHostedService(options =>
			{
				// when shutting down we want jobs to complete gracefully
				options.WaitForJobsToComplete = true;
				options.AwaitApplicationStarted = true;
			});
			// var scheduler = new StdSchedulerFactory().GetScheduler().Result;
			// scheduler.JobFactory = new DiJobFactory(services.BuildServiceProvider());
			// scheduler.Start();
			//
			// services.AddSingleton(scheduler);
		}
	}
}
