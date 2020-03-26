﻿using sdLitica.Events.Abstractions;
using sdLitica.Events.Extensions;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Bus
{
    /// <summary>
    /// This class allows to publish an event, subcribe an event or read an event
    /// </summary>
    internal class EventBus : IEventBus
    {
        private readonly IPublisher _publisher;
        private readonly IConsumer _consumer;
        private readonly IEventRegistry _eventRegistry;

        public EventBus(IPublisher publisher, IConsumer consumer, IEventRegistry eventRegistry)
        {
            _publisher = publisher;
            _consumer = consumer;
            _eventRegistry = eventRegistry;
        }

        /// <summary>
        /// To publish an event to a queue or exchange
        /// </summary>
        /// <param name="event"></param>
        public void Publish(IEvent @event)
        {
            var message = @event.ToMessage();
            var exchanges = _eventRegistry.GetPublishingTarget(@event);
            foreach (var exchange in exchanges)
            {
                _publisher.Publish(exchange, message);
            }
        }

        /// <summary>
        /// Read to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void Read<T>(Action<T> action) where T : IEvent
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subcribe to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            var type = Activator.CreateInstance<T>();
            var exchanges = _eventRegistry.GetPublishingTarget(type);

            foreach (var exchange in exchanges)
                _consumer.Subscribe(exchange, (obj) => 
                {
                    var @event = (T) obj;
                    action(@event);
                });
            
        }
    }
}
