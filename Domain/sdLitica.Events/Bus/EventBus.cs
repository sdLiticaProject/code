using sdLitica.Events.Abstractions;
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

        public EventBus(IPublisher publisher, IConsumer consumer)
        {
            _publisher = publisher;
            _consumer = consumer;
        }

        public void Publish(IEvent @event)
        {
            throw new NotImplementedException();
        }

        public void Read<T>(string eventName, Action<T> action) where T : IEvent
        {
            throw new NotImplementedException();
        }

        public void Subscribe<T>(string eventName, Action<T> action) where T : IEvent
        {
            throw new NotImplementedException();
        }
    }
}
