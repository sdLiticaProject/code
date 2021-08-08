using System;
using System.Collections.Generic;

namespace sdLitica.Events.Abstractions
{
    /// <summary>
    /// This interface enables to link an event to a queue or exchange
    /// </summary>
    public interface IEventRegistry
    {
        /// <summary>
        /// Register an event to a queue or exchange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        void Register<T>(string exchange, string exchangeType = "topic") where T: IEvent;

        /// <summary>
        /// Get publishing queue or exchange
        /// </summary>
        /// <typeparam name="T">event type</typeparam>
        /// <returns></returns>
        IList<string> GetPublishingTarget(Type eventType);
    }
}