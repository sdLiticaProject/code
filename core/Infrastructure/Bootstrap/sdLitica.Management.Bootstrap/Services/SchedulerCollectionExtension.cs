using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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

			// var scheduler = new StdSchedulerFactory().GetScheduler().Result;
			// scheduler.JobFactory = new DiJobFactory(services.BuildServiceProvider());
			// scheduler.Start();
			//
			// services.AddSingleton(scheduler);
		}
	}
}
