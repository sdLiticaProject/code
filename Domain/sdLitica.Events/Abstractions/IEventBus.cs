using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    /// <summary>
    /// This interface allows to publish an event, subcribe an event or read an event
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// To publish an event to a queue or exchange
        /// </summary>
        /// <param name="event"></param>
        /// <param name="routingKey"></param>
        void Publish(IEvent @event, string routingKey);
        /// <summary>
        /// Subcribe to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey"></param>
        /// <param name="action"></param>
        void Subscribe<T>(string routingKey, Action<T> action) where T : IEvent;
        /// <summary>
        /// Read to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Read<T>(Action<T> action) where T : IEvent;
    }
}
