using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using sdLitica.Bootstrap.Extensions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.FSharpAnalyticalModule.IntegrationEvents.EventHandling;
using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;

namespace sdLitica.FSharpAnalyticalModule
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            // Add appsettings file configuration to bootstrap
            Configuration.AddSettings();

            services.AddTransient<IAppSettings, AppSettings>();
            services.AddTransient<Program>();
            services.AddTimeSeriesDatabase();
            services.AddScoped<AnalyticsIntegrationEventHandler>();


            
            // Add event and messages support
            services.AddEventsAndMessages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
            ConfigureEventBus(app);
        }

        protected virtual void ConfigureEventBus(IApplicationBuilder app)
        {
            IEventRegistry registry = app.ApplicationServices.GetRequiredService<IEventRegistry>();
            registry.Register<TimeSeriesAnalysisRequestEvent>(Exchanges.TimeSeries);
            registry.Register<DiagnosticsResponseEvent>(Exchanges.Diagnostics);
            registry.Register<AnalyticModuleRegistrationRequestEvent>(Exchanges.ModuleRegistrations);

            using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            {
                IEventBus eventBus = scope.ServiceProvider.GetService<IEventBus>();

                eventBus.SubscribeToTopic<TimeSeriesAnalysisRequestEvent>(async (@event) =>
                {
                    using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
                    {
                        AnalyticsIntegrationEventHandler handler = scope.ServiceProvider.GetRequiredService<AnalyticsIntegrationEventHandler>();
                        await handler.Handle(@event, "Min");
                    }
                }, "*.min");

                eventBus.SubscribeToTopic<TimeSeriesAnalysisRequestEvent>(async (@event) =>
                {
                    using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
                    {
                        AnalyticsIntegrationEventHandler handler = scope.ServiceProvider.GetRequiredService<AnalyticsIntegrationEventHandler>();
                        await handler.Handle(@event, "Mean");
                    }
                }, "*.mean");

                eventBus.SubscribeToTopic<TimeSeriesAnalysisRequestEvent>(async (@event) =>
                {
                    using (IServiceScope scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
                    {
                        AnalyticsIntegrationEventHandler handler = scope.ServiceProvider.GetRequiredService<AnalyticsIntegrationEventHandler>();
                        await handler.Handle(@event, "Max");
                    }
                }, "*.max");
            }
        }

    }
}
