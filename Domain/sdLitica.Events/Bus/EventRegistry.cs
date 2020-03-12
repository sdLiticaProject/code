using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using sdLitica.Messages.Abstractions;

namespace sdLitica.Events.Bus
{
    public class EventRegistry : IEventRegistry
    {
        private readonly IDictionary<Type, IList<string>> _eventRegistry;
        private readonly IBrokerManager _brokerManager;

        public EventRegistry(IBrokerManager brokerManager)
        {
            _eventRegistry = new Dictionary<Type, IList<string>>();
            _brokerManager = brokerManager;
        }

        public void Register<T>(string queue) where T : IEvent
        {
            var eventType = typeof(T);

            if (_eventRegistry.ContainsKey(eventType))
            {
                if (!_eventRegistry[eventType].Contains(queue))
                    _eventRegistry[eventType].Add(queue);
                return;
            }

            _eventRegistry.Add(new KeyValuePair<Type, IList<string>>(eventType, new List<string>() { queue }));
            
            //TODO needs: create queue OR exchanges
            _brokerManager.CreateQueue(queue);
        }

        public IList<string> GetExchangesForEvent<T>(T @event) where T : IEvent
        {
            var eventType = @event.GetType();

            if (!_eventRegistry.ContainsKey(eventType)) return null;

            return _eventRegistry[eventType];
        }        
    }
}
