using System;
using System.Threading.Tasks;

namespace sdLitica.Events.Abstractions
{
    /// <summary>
    /// This interface allows to publish an event, subscribe an event or read an event
    /// </summary>
    public interface IEventBus
    {
        /// <summary>
        /// To publish (direct) an event to a queue or exchange
        /// </summary>
        /// <param name="event"></param>
        void Publish(IEvent @event);

        /// <summary>
        /// To publish (topic) an event to a queue or exchange
        /// </summary>
        /// <param name="event"></param>
        /// <param name="routingKey"></param>
        void PublishToTopic(IEvent @event, string routingKey = "");

        /// <summary>
        /// Subscribe (topic) to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="routingKey"></param>
        /// <param name="action"></param>
        void SubscribeToTopic<T>(Action<T> action, string routingKey = "") where T: IEvent;

        /// <summary>
        /// Subscribe (direct) to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Subscribe<T>(Action<T> action) where T: IEvent;

        /// <summary>
        /// Subscribe (direct) to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Subscribe<T>(Func<T, Task> action) where T: IEvent;

        /// <summary>
        /// Read to an event and run the action after receive it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        void Read<T>(Action<T> action) where T: IEvent;
    }
}