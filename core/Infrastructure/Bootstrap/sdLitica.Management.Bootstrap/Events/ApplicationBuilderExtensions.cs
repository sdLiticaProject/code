using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.AnalyticsManagementCore;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
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
            IEventRegistry registry = app.ApplicationServices.GetRequiredService<IEventRegistry>();
            
            registry.Register<TimeSeriesAnalysisRequestEvent>(Exchanges.TimeSeries);
            registry.Register<DiagnosticsResponseEvent>(Exchanges.Diagnostics);
            registry.Register<AnalyticModuleRegistrationRequestEvent>(Exchanges.ModuleRegistrations);

            DiagnosticsListener.Services = app.ApplicationServices.GetRequiredService<IServiceProvider>();
            
            using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            {
                IEventBus eventBus = scope.ServiceProvider.GetRequiredService<IEventBus>();
                AnalyticsRegistry analyticsRegistry = scope.ServiceProvider.GetRequiredService<AnalyticsRegistry>();
                AnalyticsOperationRequestRepository operationRequestRepository = scope.ServiceProvider.GetRequiredService<AnalyticsOperationRequestRepository>();

                DiagnosticsListener.Initialize(registry, eventBus, operationRequestRepository, analyticsRegistry);
                DiagnosticsListener.Listen();
                DiagnosticsListener.ListenNewModules();

            }
        }
    }
}