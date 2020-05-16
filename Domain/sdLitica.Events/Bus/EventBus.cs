using sdLitica.Events.Abstractions;
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
            IMessage message = @event.ToMessage();
            IList<string> exchanges = _eventRegistry.GetPublishingTarget(@event);
            foreach (var exchange in exchanges)
            {
                _publisher.Publish(exchange, message);
            }
        }

        /// <summary>
        /// To publish (topic) an event to a queue or exchange
        /// </summary>
        /// <param name="event"></param>
        public void PublishToTopic(IEvent @event, string routingKey="basic")
        {
            IMessage message = @event.ToMessage();
            IList<string> exchanges = _eventRegistry.GetPublishingTarget(@event);
            foreach (var exchange in exchanges)
            {
                _publisher.PublishToTopic(exchange, routingKey, message);
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
        /// Subcribe (topic) to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void SubscribeToTopic<T>(Action<T> action, string routingKey="basic") where T : IEvent
        {
            T type = Activator.CreateInstance<T>();
            IList<string> exchanges = _eventRegistry.GetPublishingTarget(type);

            foreach (var exchange in exchanges)
                _consumer.SubscribeToTopic(exchange, routingKey, (obj) => 
                {
                    T @event = (T) obj;
                    action(@event);
                });
            
        }

        /// <summary>
        /// Subcribe to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            T type = Activator.CreateInstance<T>();
            IList<string> exchanges = _eventRegistry.GetPublishingTarget(type);

            foreach (var exchange in exchanges)
                _consumer.Subscribe(exchange, (obj) =>
                {
                    T @event = (T)obj;
                    action(@event);
                });

        }
    }
}
