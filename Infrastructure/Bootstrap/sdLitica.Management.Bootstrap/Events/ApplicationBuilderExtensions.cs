using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Events.Bus;
using sdLitica.TimeSeries.Services;
using System.Threading.Tasks;
using Analysis;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

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
            var timeSeriesService = app.ApplicationServices.GetRequiredService<ITimeSeriesService>();

            registry.Register<TimeSeriesAnalysisEvent>(Exchanges.TimeSeries);
            registry.Register<FSharpTimeSeriesAnalysisEvent>(Exchanges.FSharpTimeSeries);
           


            using (var scope = app.ApplicationServices.GetRequiredService<IServiceProvider>().CreateScope())
            {
                var sampleBus = scope.ServiceProvider.GetRequiredService<IEventBus>();

                sampleBus.Subscribe<TimeSeriesAnalysisEvent>((TimeSeriesAnalysisEvent @event) =>
                {
                    Console.WriteLine(@event.Id + " " + @event.Name);
                });

                sampleBus.Subscribe<FSharpTimeSeriesAnalysisEvent>((FSharpTimeSeriesAnalysisEvent @event) =>
                {
                    Console.WriteLine("processing " + @event.Name);
                    Task<InfluxResult<DynamicInfluxRow>> task = timeSeriesService.ReadMeasurementById(@event.Name);
                    task.Wait();
                    List<DynamicInfluxRow> rows = task.Result.Series[0].Rows;
                    double[] series = new double[rows.Count];
                    for (int i = 0; i < rows.Count; i++)
                        series[i] = (double)rows[i].Fields["cpu"];
                    double d = ExampleFunctions.Mean(series);
                    Console.WriteLine(@event.Id + " " + @event.Name + " - mean_cpu = " + d);
                });
            }
        }
    }
}