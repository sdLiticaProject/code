using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Analytics;
using sdLitica.Bootstrap.Extensions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.TimeSeries.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;
using log4net;
using log4net.Config;
using System.Reflection;
using sdLitica.Utils.Models;
using System.Threading;

namespace sdLitica.ExampleDaemonManagement
{
    /// <summary>
    /// Simple example of analytical module. 
    /// </summary>
    class Program
    {
        // define a logger
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));

        static void Main(string[] args)
        {
            // setup a configuration for logs
            BasicConfigurator.Configure(LogManager.GetRepository(Assembly.GetEntryAssembly()));

            // configure dependency injection
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();

            var _timeSeriesService = serviceProvider.GetRequiredService<ITimeSeriesService>();
            var registry = serviceProvider.GetRequiredService<IEventRegistry>();

            // register events
            registry.Register<TimeSeriesAnalysisRequest>(Exchanges.TimeSeries);
            registry.Register<DiagnosticsResponse>(Exchanges.Diagnostics);
            registry.Register<AnalyticModuleRegistrationRequest>(Exchanges.ModuleRegistration);

            // creating module's model
            var opArr = new List<AnalyticsOperationModel>();
            opArr.Add(new AnalyticsOperationModel() { Name = "Mean", Description = "Average" });
            AnalyticsModuleRegistrationModel moduleModel = new AnalyticsModuleRegistrationModel()
            {
                ModuleGuid = Guid.NewGuid(),
                QueueName = "mean_module",
                Operations = opArr
            };

            using (var scope = serviceProvider.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var sampleBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                var peepTimer = new Timer((e) => { SendPeep(sampleBus, moduleModel); }, null, 5000, 5000); // todo: remove hardcoded values
                

                // subscribe to analytical operations
                sampleBus.SubscribeToTopic<TimeSeriesAnalysisRequest>((TimeSeriesAnalysisRequest @event) =>
                {
                    log.Info(@event.Name + " " + @event.Operation.OpName);
                    AnalyticsOperationRequest operation = @event.Operation;
                    log.Info(operation.Id);

                    try
                    {
                        // get time-series given by TimeSeriesId and extract some data from it
                        Task<InfluxResult<DynamicInfluxRow>> task = _timeSeriesService.ReadMeasurementById(@event.Operation.TimeSeriesId);
                        task.Wait();
                        List<DynamicInfluxRow> rows = task.Result.Series[0].Rows;
                        double[] series = new double[rows.Count];
                        for (int i = 0; i < rows.Count; i++)
                            series[i] = (double)rows[i].Fields["cpu"];

                        // invoke method (from F-sharp lib) given by OpName.
                        log.Info(typeof(ExampleDaemonAnalysis.ExampleFunctions).GetMethod(@event.Operation.OpName).Invoke(null, new[] { series }));

                        // if no errors, status is complete
                        operation.Status = OperationStatus.Complete;

                    }
                    catch (Exception e)
                    {
                        // if any error, set status
                        operation.Status = OperationStatus.Error;
                    }
                    finally
                    {
                        // publish information about operation
                        sampleBus.Publish(new DiagnosticsResponse(operation));
                    }
                }, "mean_module");
                
                
            }

    

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        public static void SendPeep(IEventBus sampleBus, AnalyticsModuleRegistrationModel moduleModel)
        {
            sampleBus.Publish(new AnalyticModuleRegistrationRequest(moduleModel));
        }

        /// <summary>
        /// Loads configuration for dependency injection.
        /// </summary>
        /// <returns></returns>
        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

        /// <summary>
        /// configure dependency injection
        /// </summary>
        /// <returns></returns>
        private static IServiceCollection ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();

            var config = LoadConfiguration();
            services.AddSingleton(config);

            // required to run the application
            services.AddTransient<Program>();



            // Add appsettings file configuration to bootstrap
            config.AddSettings();

            // Add any type of services available
            services.AddServices();

            // Add relational database support
            services.AddRelationalDatabase();

            // Add time series support
            services.AddTimeSeriesDatabase();

            // Add event and messages support
            services.AddEventsAndMessages();

            return services;
        }

    }
}
