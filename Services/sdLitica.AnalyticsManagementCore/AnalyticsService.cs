using System;
using sdLitica.Analytics;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Relational.Repositories;

namespace sdLitica.AnalyticsManagementCore
{
    /// <summary>
    /// This class provides services for analytics operations.
    /// </summary>
    public class AnalyticsService
    {
        private readonly IEventBus _eventBus;
        private readonly OperationRepository _operationRepository;

        public AnalyticsService(IEventBus eventBus, OperationRepository operationRepository)
        {
            _eventBus = eventBus;
            _operationRepository = operationRepository;
        }

        /// <summary>
        /// Execute operation. Publishes event, that contains AnalyticsOperation for analytical modules.
        /// </summary>
        /// <param name="operation"></param>
        public void ExecuteOperation(AnalyticsOperationRequest operation)
        {
            operation.SetId(); // todo: separate json-model from entity and set id during creation of entity

            _operationRepository.Add(operation);     

            TimeSeriesAnalysisRequest @event = new TimeSeriesAnalysisRequest(operation);
            _eventBus.Publish(@event, "basic");

            _operationRepository.SaveChanges();
        }

        /// <summary>
        /// Check status of operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public OperationStatus CheckResults(AnalyticsOperationRequest operation)
        {
            return _operationRepository.GetStatus(operation.Id);
        }

        /// <summary>
        /// Fetch results of operation.
        /// </summary>
        /// <param name="operation"></param>
        public void FetchResults(AnalyticsOperationRequest operation)
        {
            throw new NotImplementedException();
        }
    }
}
