using System;
using System.Collections;
using System.Collections.Generic;
using sdLitica.Analytics;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Exceptions.Http;
using sdLitica.Relational.Repositories;
using sdLitica.Utils.Models;

namespace sdLitica.AnalyticsManagementCore
{
    /// <summary>
    /// This class provides services for analytics operations.
    /// </summary>
    public class AnalyticsService
    {
        private readonly IEventBus _eventBus;
        private readonly OperationRequestRepository _OperationRequestRepository;
        private readonly AnalyticsRegistry _analyticsRegistry;

        public AnalyticsService(IEventBus eventBus, OperationRequestRepository OperationRequestRepository, AnalyticsRegistry analyticsRegistry)
        {
            _eventBus = eventBus;
            _OperationRequestRepository = OperationRequestRepository;
            _analyticsRegistry = analyticsRegistry;
        }

        /// <summary>
        /// Execute operation. Publishes event, that contains AnalyticsOperation for analytical modules.
        /// </summary>
        /// <param name="operation"></param>
        public void ExecuteOperation(AnalyticsOperationRequest operation)
        {

            _OperationRequestRepository.Add(operation);
            _OperationRequestRepository.SaveChanges();

            TimeSeriesAnalysisRequest @event = new TimeSeriesAnalysisRequest(operation);
            string routingKey = _analyticsRegistry.GetQueue(operation.OpName);
            if (routingKey != null)
            {
                _eventBus.PublishToTopic(@event, routingKey);
            }
            else
            {
                throw new InvalidRequestException("no such operation"); // not sure if it is right exception
            }
        }

        /// <summary>
        /// Get list of available operations
        /// </summary>
        /// <returns></returns>
        public IList<AnalyticsOperationModel> GetAvailableOperations()
        {
            return _analyticsRegistry.GetAvailableOperations();
        }

        /// <summary>
        /// Check status of operation.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public OperationStatus CheckResults(AnalyticsOperationRequest operation)
        {
            return _OperationRequestRepository.GetStatus(operation.Id);
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
