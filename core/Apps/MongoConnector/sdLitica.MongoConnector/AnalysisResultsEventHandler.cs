using System;
using Microsoft.Extensions.Logging;
using sdLitica.AnalysisResults.Model;
using sdLitica.AnalysisResults.Repositories;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;

namespace sdLitica.MongoConnector
{
    public class AnalysisResultsEventHandler
    {
        private readonly IAnalysisResultsRepository _repository;
        private readonly ILogger<AnalysisResultsEventHandler> _logger;

        public AnalysisResultsEventHandler(
            IAnalysisResultsRepository repository,
            ILogger<AnalysisResultsEventHandler> logger
        )
        {
            _repository = repository;
            _logger = logger;
        }

        public void HandleNewAnalysisResult(NewAnalysisResultEvent @event)
        {
            var result = new AnalysisResult
            {
                Id = new Guid(),
                TimeSeriesId = @event.TimeSeriesId,
                DateCreated = DateTime.UtcNow,
                Type = @event.Type,
                Result = @event.Result
            };
            _logger.LogDebug(
                "Add new analysis result: Id={Id}, timeseriesId={TsId}, type={Type}",
                result.Id, result.TimeSeriesId, result.Type
            );
            _repository.Add(result);
        }

        public IEvent HandleAnalysisResultRequest(IEvent @event)
        {
            var request = @event as AnalysisResultRequestEvent
                          ?? throw new ArgumentException(
                              $"Received {@event.GetType()}, but should be {nameof(AnalysisResultRequestEvent)}", nameof(@event)
                          );
            var result = _repository.GetById(request.AnalysisResultId);
            return new AnalysisResultResponseEvent(result);
        }
    }
}