using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// Background interface to publish any event to a queue or exchange
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publish this message to a queue or exchange
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="message"></param>
        void Publish(string exchange, string routingKey, IMessage message);
    }
}
