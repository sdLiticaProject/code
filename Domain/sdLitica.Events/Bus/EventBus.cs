using sdLitica.Events.Abstractions;
using sdLitica.Events.Extensions;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Bus
{
    public class EventBus : IEventBus
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

        public void Publish(IEvent @event)
        {
            var message = @event.ToMessage();
            var exchanges = _eventRegistry.GetExchangesForEvent(@event);
            foreach (var exchange in exchanges)
                _publisher.Publish(exchange, message);
        }

        public void Read<T>(string eventName, Action<T> action) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(string eventName, Action<T> action) where T : IEvent
        {
            var exchanges = _eventRegistry.GetExchangesForEvent<T>();

            foreach (var exchange in exchanges)
                _consumer.Subscribe(exchange, (obj) => 
                {
                    var @event = (T) obj;
                    action(@event);
                });
            
        }
    }
}
