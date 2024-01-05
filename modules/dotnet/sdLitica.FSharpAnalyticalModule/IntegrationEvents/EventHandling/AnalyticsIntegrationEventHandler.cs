﻿using Deedle;
using InfluxDB.Client.Core.Flux.Domain;
using sdLitica.Entities.Analytics;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.TimeSeries.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Vibrant.InfluxDB.Client;
//using Vibrant.InfluxDB.Client.Rows;

namespace sdLitica.FSharpAnalyticalModule.IntegrationEvents.EventHandling
{

    public class AnalyticsIntegrationEventHandler
    {

        static ITimeSeriesService _timeSeriesService;
        private static IBucketMetadataService _bucketMetadataService;
        static IEventBus _eventBus;
        public AnalyticsIntegrationEventHandler(
            ITimeSeriesService timeSeriesService,
            IBucketMetadataService bucketMetadataService,
            IEventBus eventBus)
        {
            _timeSeriesService = timeSeriesService;
            _bucketMetadataService = bucketMetadataService;
            _eventBus = eventBus;
        }
        public async Task Handle(TimeSeriesAnalysisRequestEvent @event, string operationName)
        {
            UserAnalyticsOperation operation = @event.Operation;
            
            try
            {
                List<FluxTable> influxResult =
                    await _timeSeriesService.ReadMeasurementById(operation.TimeSeriesId, _bucketMetadataService.GetBucketMetadata(operation.TimeSeriesId).InfluxId);
                //List<DynamicInfluxRow> rowsList = influxResult.Series[0].Rows;
                var rowsList = influxResult[0].Records;
                Series<DateTime?, double> series = rowsList
                    .Select(x => KeyValue.Create(
                        x.GetTimeInDateTime(),
                        (double)x.GetValueByKey(operation.Arguments["column"].ToString()))).
                        //(double)x.Fields[operation.Arguments["column"].ToString()])).
                    ToSeries();

                // apply operation
                switch (operationName)
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
