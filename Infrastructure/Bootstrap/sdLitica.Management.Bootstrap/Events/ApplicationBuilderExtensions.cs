using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Events.Bus;

namespace sdLitica.Bootstrap.Events
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Sample event subsribe: Register + Subscribe
        /// </summary>
        /// <param name="app"></param>
        public static void SubscribeEvents(this IApplicationBuilder app)
        {
            var registry = app.ApplicationServices.GetRequiredService<IEventRegistry>();
            
            registry.Register<TimeSeriesAnalysisEvent>(Exchanges.TimeSeries);

            using (var scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var sampleBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                sampleBus.Subscribe<TimeSeriesAnalysisEvent>((TimeSeriesAnalysisEvent @event) =>
                {
                    Console.WriteLine(@event.Id + " " + @event.Name);
                });
            }
        }
    }
}