using sdLitica.Events.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Bus
{
    public class EventBus : IEventBus
    {
        public EventBus()
        {

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
