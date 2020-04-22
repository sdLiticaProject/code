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
        public void ExecuteOperation(AnalyticsOperation operation)
        {
            operation.SetId(); // todo: separate json-model from entity and set id during creation of entity

            _operationRepository.Add(operation);     

            TimeSeriesAnalysisEvent @event = new TimeSeriesAnalysisEvent(operation);
            _eventBus.Publish(@event);

            _operationRepository.SaveChanges();
        }

        /// <summary>
        /// Check status of operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public OperationStatus CheckResults(AnalyticsOperation operation)
        {
            return _operationRepository.GetStatus(operation.Id);
        }

        /// <summary>
        /// Fetch results of operation.
        /// </summary>
        /// <param name="operation"></param>
        public void FetchResults(AnalyticsOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}
