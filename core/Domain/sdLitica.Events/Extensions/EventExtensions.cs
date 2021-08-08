using System;
using System.Reflection;
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
        
        public static IEvent ToEvent(this byte[] body)
        {
            string strMessage = Encoding.UTF8.GetString(body);

            Message message = JsonConvert.DeserializeObject<Message>(strMessage)
                              ?? throw new Exception("Could not deserialize message object");

            Assembly eventAssembly = Assembly.Load("sdLitica.Events");
            Type type = eventAssembly.GetType(message.Type)
                        ?? throw new Exception("Could not find corresponding type for event");
            return (IEvent) JsonConvert.DeserializeObject(message.Body, type);
        }

        public static byte[] ToBytes(this IEvent @event)
        {
            string content = JsonConvert.SerializeObject(@event.ToMessage());
            return Encoding.UTF8.GetBytes(content);
        }
    }
}
