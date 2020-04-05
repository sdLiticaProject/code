using System;
using sdLitica.Analytics.Abstractions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Messages.Abstractions;
using sdLitica.Relational.Repositories;

namespace sdLitica.AnalyticsManagementCore
{
    public class AnalyticsService
    {
        private readonly IEventBus _eventBus;
        private readonly OperationRepository _operationRepository;

        public AnalyticsService(IEventBus eventBus, OperationRepository operationRepository)
        {
            _eventBus = eventBus;
            _operationRepository = operationRepository;
        }

        public void ExecuteOperation(AnalyticsOperation operation)
        {
            operation.SetId();
            _operationRepository.Add(operation);     
            TimeSeriesAnalysisEvent @event = new TimeSeriesAnalysisEvent(operation);
            _eventBus.Publish(@event);
            _operationRepository.SaveChanges();
        }

        public int CheckResults(AnalyticsOperation operation)
        {
            return _operationRepository.GetStatus(operation.Id);
        }

        public void FetchResults(IAnalyticsOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}
