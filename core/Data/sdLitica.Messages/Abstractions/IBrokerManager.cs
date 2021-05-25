using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// Interface to create exchange or queues
    /// </summary>
    public interface IBrokerManager
    {
        /// <summary>
        /// Create exchange in the broker
        /// </summary>
        /// <param name="name"></param>
        /// <param name="exchangeType"></param>
        void CreateExchange(string name, string exchangeType);
        /// <summary>
        /// Create queue in the broker
        /// </summary>
        /// <param name="name"></param>
        void CreateQueue(string name);        
    }
}
