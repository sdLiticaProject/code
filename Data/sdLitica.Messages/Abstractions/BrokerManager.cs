﻿using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// This class manages Queue and Exchanges creation
    /// </summary>
    internal class BrokerManager : IBrokerManager
    {
        private readonly BrokerConnection _brokerConnection;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="brokerConnection"></param>
        public BrokerManager(BrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;
        }
        
        /// <summary>
        /// Create exchange
        /// </summary>
        /// <param name="name"></param>
        public void CreateExchange(string name)
        {
            _brokerConnection.CreateExchange(name, ExchangeType.Topic);
        }

        /// <summary>
        /// Create queue
        /// </summary>
        /// <param name="name"></param>
        public void CreateQueue(string name)
        {
            _brokerConnection.CreateQueue(name);
        }
    }
}
