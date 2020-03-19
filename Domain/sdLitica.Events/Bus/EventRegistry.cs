﻿using sdLitica.Events.Abstractions;
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
       
        internal EventRegistry(IBrokerManager brokerManager)
        {
            _eventRegistry = new Dictionary<Type, IList<string>>();
            _brokerManager = brokerManager;
        }

        /// <summary>
        /// Register an event to a queue or exchange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange"></param>
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

        /// <summary>
        /// Get publishing queue or exchange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        public IList<string> GetPublishingTarget<T>(T @event) where T : IEvent
        {
            var eventType = @event.GetType();

            if (!_eventRegistry.ContainsKey(eventType)) return null;

            return _eventRegistry[eventType];
        }        
    }
}
