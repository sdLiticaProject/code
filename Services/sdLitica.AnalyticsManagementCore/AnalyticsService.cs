using System;
using sdLitica.Analytics.Abstractions;
using sdLitica.Events.Abstractions;
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

        public void ExecuteOperation(IAnalyticsOperation operation)
        {
            AnalyticsOperationEvent @event = new AnalyticsOperationEvent(operation);
            _eventBus.Publish(@event);
        }
    }
}
