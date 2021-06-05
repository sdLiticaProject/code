using System;
using System.Threading.Tasks;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// Interface to describe consumer operation
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// Read a exchange
        /// </summary>
        /// <param name="queue"></param>
        void Read(string queue);

        /// <summary>
        /// Subscribe (topic) a exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="action"></param>
        void SubscribeToTopic(string exchange, string routingKey, Action<object> action);

        /// <summary>
        /// Subscribe a exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="action"></param>
        void Subscribe(string exchange, Action<object> action);

        /// <summary>
        /// Subscribe a exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="action"></param>
        void Subscribe(string exchange, Func<object, Task> action);
    }
}