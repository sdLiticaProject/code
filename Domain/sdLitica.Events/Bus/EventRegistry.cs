using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using sdLitica.Messages.Abstractions;

namespace sdLitica.Events.Bus
{
    /// <summary>
    /// This class enables to link an event to a queue or exchange
    /// </summary>
    internal class EventRegistry : IEventRegistry
    {
        private readonly IDictionary<Type, IList<string>> _eventRegistry;
        private readonly IBrokerManager _brokerManager;
       
        public EventRegistry(IBrokerManager brokerManager)
        {
            _eventRegistry = new Dictionary<Type, IList<string>>();
            _brokerManager = brokerManager;
        }

        /// <summary>
        /// Register an event to a queue or exchange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public void Register<T>(string exchange, string exchangeType="topic") where T : IEvent
        {
            Type eventType = typeof(T);

            if (_eventRegistry.ContainsKey(eventType))
            {
                if (!_eventRegistry[eventType].Contains(exchange))
                    _eventRegistry[eventType].Add(exchange);
                return;
            }

            _eventRegistry.Add(new KeyValuePair<Type, IList<string>>(eventType, new List<string>() { exchange }));
            
            _brokerManager.CreateExchange(exchange, exchangeType);
        }

        /// <summary>
        /// Get publishing queue or exchange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public IList<string> GetPublishingTarget<T>(T @event) where T : IEvent
        {
            Type eventType = @event.GetType();

            if (!_eventRegistry.ContainsKey(eventType)) return null;

            return _eventRegistry[eventType];
        }        
    }
}
