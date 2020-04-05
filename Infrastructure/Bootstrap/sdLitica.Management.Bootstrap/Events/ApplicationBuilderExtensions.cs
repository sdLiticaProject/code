using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Events.Bus;
using sdLitica.AnalyticsManagementCore;
using sdLitica.Relational.Repositories;

namespace sdLitica.Bootstrap.Events
{
    /// <summary>
    /// Application Builder Extensions. Method here provides a sample
    /// </summary>
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
            registry.Register<DiagnosticsEvent>(Exchanges.Diagnostics);

            DiagnosticsListener._services = app.ApplicationServices.GetRequiredService<IServiceProvider>();
            
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                var operationRepository = scope.ServiceProvider.GetRequiredService<OperationRepository>();

                DiagnosticsListener.Initialize(registry, eventBus, operationRepository);
                DiagnosticsListener.Listen();
            }
        }
    }
}