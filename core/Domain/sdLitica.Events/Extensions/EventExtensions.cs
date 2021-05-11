using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using sdLitica.Events.Abstractions;
using sdLitica.Messages.Abstractions;

namespace sdLitica.Events.Extensions
{
    /// <summary>
    /// Event extensions
    /// </summary>
    public static class EventExtensions
    {
        /// <summary>
        /// Convert from event to message
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public static IMessage ToMessage(this IEvent @event)
        {

            string type = @event?.GetType().FullName ?? throw new ArgumentNullException($"The event cannot be null");
            string body = JsonConvert.SerializeObject(@event);

            return new Message(type, body);
        }
    }
}
