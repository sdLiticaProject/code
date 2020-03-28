using System;
using sdLitica.Analytics.Abstractions;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Integration;
using sdLitica.Messages.Abstractions;

namespace sdLitica.AnalyticsManagementCore
{
    public class AnalyticsService
    {
        private readonly IEventBus _eventBus;

        public AnalyticsService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public void ExecuteOperation(AnalyticsOperation operation)
        {
            TimeSeriesAnalysisEvent @event = new TimeSeriesAnalysisEvent(operation);
            _eventBus.Publish(@event);
        }

        public void CheckResults(IAnalyticsOperation operation)
        {
            throw new NotImplementedException();
        }

        public void FetchResults(IAnalyticsOperation operation)
        {
            throw new NotImplementedException();
        }
    }
}
