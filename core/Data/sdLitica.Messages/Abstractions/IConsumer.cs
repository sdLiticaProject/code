using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// Interface to describe consumer operation
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// Read a queue
        /// </summary>
        /// <param name="queue"></param>
        void Read(string queue);
        /// <summary>
        /// Subscribe (topic) a queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="routingKey"></param>
        /// <param name="action"></param>
        void SubscribeToTopic(string queue, string routingKey, Action<object> action);
        /// <summary>
        /// Subscribe a queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="action"></param>
        void Subscribe(string queue, Action<object> action);

        /// <summary>
        /// Subscribe a queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="action"></param>
        void Subscribe(string queue, Func<object, Task> action);
    }
}
