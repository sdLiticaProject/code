using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Bootstrap.Extensions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Integration;
using sdLitica.Messages.Abstractions;
using sdLitica.TimeSeries.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.ExampleDaemonManagement
{
    class Program
    {
        static void Main(string[] args)
        {

            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var _timeSeriesService = serviceProvider.GetRequiredService<ITimeSeriesService>();



            var registry = serviceProvider.GetRequiredService<IEventRegistry>();

            registry.Register<TimeSeriesAnalysisEvent>(Exchanges.TimeSeries);


            using (var scope = serviceProvider.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var sampleBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                
                sampleBus.Subscribe<TimeSeriesAnalysisEvent>((TimeSeriesAnalysisEvent @event) =>
                {
                    Console.WriteLine(@event.Name + " " + @event.Operation.OpName);

                    Task<InfluxResult<DynamicInfluxRow>> task = _timeSeriesService.ReadMeasurementById(@event.Operation.TsId);
                    task.Wait();
                    List<DynamicInfluxRow> rows = task.Result.Series[0].Rows;
                    double[] series = new double[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                        series[i] = (double)rows[i].Fields["cpu"];

                    Console.WriteLine(typeof(ExampleDaemonAnalysis.ExampleFunctions).GetMethod(@event.Operation.OpName).Invoke(null, new[] { series }));
                });
                
            }

    

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }

        public static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

            return builder.Build();
        }

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
