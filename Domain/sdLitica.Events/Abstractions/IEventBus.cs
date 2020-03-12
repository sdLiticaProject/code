using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    public interface IEventBus
    {
        void Publish(IEvent @event);
        void Subscribe<T>(Action<T> action) where T : IEvent;
        void Read<T>(Action<T> action) where T : IEvent;
    }
}
