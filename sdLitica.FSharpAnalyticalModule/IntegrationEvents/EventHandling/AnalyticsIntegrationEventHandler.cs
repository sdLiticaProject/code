using Deedle;
using sdLitica.Entities.Analytics;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.TimeSeries.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vibrant.InfluxDB.Client;
using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.FSharpAnalyticalModule.IntegrationEvents.EventHandling
{

    public class AnalyticsIntegrationEventHandler
    {

        static ITimeSeriesService _timeSeriesService;
        static IEventBus _eventBus;
        public AnalyticsIntegrationEventHandler(
            ITimeSeriesService timeSeriesService,
            IEventBus eventBus)
        {
            _timeSeriesService = timeSeriesService;
            _eventBus = eventBus;
        }
        public async Task Handle(TimeSeriesAnalysisRequestEvent @event)
        {
            UserAnalyticsOperation operation = @event.Operation;
            
            try
            {
                InfluxResult<DynamicInfluxRow> influxResult =
                    await _timeSeriesService.ReadMeasurementById(operation.TimeSeriesId);
                List<DynamicInfluxRow> rowsList = influxResult.Series[0].Rows;
                Series<DateTime?, double> series = rowsList
                    .Select(x => KeyValue.Create(
                        x.Timestamp,
                        (double)x.Fields[operation.Arguments["column"].ToString()])).
                    ToSeries();

                // apply operation
                switch (operation.OperationName)
                {
                    case "Mean":
                        System.Console.WriteLine(series.Mean());
                        break;
                    case "Min":
                        System.Console.WriteLine(series.Min());
                        break;
                    case "Max":
                        System.Console.WriteLine(series.Max());
                        break;
                    default:
                        throw new NotSupportedException();
                }

                operation.Status = OperationStatus.Complete;
            }
            catch (Exception e)
            {
                operation.Status = OperationStatus.Error;
            }
            finally
            {
                _eventBus.Publish(new DiagnosticsResponseEvent(operation));
            }
        }
    }
}
