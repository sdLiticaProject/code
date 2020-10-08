using System;
using System.Collections;
using System.Collections.Generic;
using sdLitica.Entities.Analytics;
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
        private readonly AnalyticsOperationRequestRepository _OperationRequestRepository;
        private readonly AnalyticsRegistry _analyticsRegistry;

        public AnalyticsService(IEventBus eventBus, AnalyticsOperationRequestRepository OperationRequestRepository, AnalyticsRegistry analyticsRegistry)
        {
            _eventBus = eventBus;
            _OperationRequestRepository = OperationRequestRepository;
            _analyticsRegistry = analyticsRegistry;
        }

        /// <summary>
        /// Execute operation. Publishes event, that contains AnalyticsOperation for analytical modules.
        /// </summary>
        /// <param name="operation"></param>
        public void ExecuteOperation(UserAnalyticsOperation operation)
        {

            _OperationRequestRepository.Add(operation);
            _OperationRequestRepository.SaveChanges();

            TimeSeriesAnalysisRequestEvent @event = new TimeSeriesAnalysisRequestEvent(operation);
            string routingKey = _analyticsRegistry.GetQueue(operation.OperationName);
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
        public OperationStatus CheckResults(UserAnalyticsOperation operation)
        {
            return _OperationRequestRepository.GetStatus(operation.Id);
        }

        /// <summary>
        /// Fetch results of operation.
        /// </summary>
        /// <param name="operation"></param>
        public void FetchResults(UserAnalyticsOperation operation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get user's analytical operation by id
        /// </summary>
        /// <param name="userAnalyticsOperationId"></param>
        /// <returns></returns>
        public UserAnalyticsOperation GetUserAnalyticsOperation(string userAnalyticsOperationId)
        {
            return _OperationRequestRepository.GetById(new Guid(userAnalyticsOperationId));
        }

        public List<UserAnalyticsOperation> GetUserOperations()
        {
            return _OperationRequestRepository.GetAll();
        }
    }
}
