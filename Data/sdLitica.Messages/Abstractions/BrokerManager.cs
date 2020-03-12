using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public class BrokerManager : IBrokerManager
    {
        private readonly BrokerConnection _brokerConnection;

        public BrokerManager(BrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;
        }
        
        public void CreateExchange(string name)
        {
            _brokerConnection.CreateExchange(name, ExchangeType.Direct);
        }

        public void CreateQueue(string name)
        {
            _brokerConnection.CreateQueue(name);
        }
    }
}
