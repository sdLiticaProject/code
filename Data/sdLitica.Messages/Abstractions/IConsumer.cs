using System;
using System.Collections.Generic;
using System.Text;

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
        /// Subscribe a queue
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="action"></param>
        void Subscribe(string queue, Action<object> action);
    }
}
