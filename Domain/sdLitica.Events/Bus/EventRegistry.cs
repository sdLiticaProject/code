using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace sdLitica.Events.Bus
{
    public class EventRegistry : IEventRegistry
    {
        private readonly IDictionary<Type, IList<string>> _eventRegistry;
                   
        public EventRegistry()
        {
            _eventRegistry = new Dictionary<Type, IList<string>>();
        }

        public void Register<T>(string exchange) where T : IEvent
        {
            var eventType = typeof(T);

            if (_eventRegistry.ContainsKey(eventType))
            {
                if (!_eventRegistry[eventType].Contains(exchange))
                    _eventRegistry[eventType].Add(exchange);
                return;
            }

            _eventRegistry.Add(new KeyValuePair<Type, IList<string>>(eventType, new List<string>() { exchange }));
        }

        public IList<string> GetExchangesForEvent<T>() where T : IEvent
        {
            var eventType = typeof(T);

            if (!_eventRegistry.ContainsKey(eventType)) return null;

            return _eventRegistry[eventType];
        }
    }
}
