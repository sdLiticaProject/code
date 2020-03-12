using Newtonsoft.Json;
using sdLitica.Events.Abstractions;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Extensions
{
    public static class EventExtensions
    {
        public static IMessage ToMessage(this IEvent @event)
        {

            var type = @event?.GetType().FullName ?? throw new ArgumentNullException($"The event cannot be null");
            var body = JsonConvert.SerializeObject(@event);

            return new Message(type, body);
        }
    }
}
