using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Consumers
{
    public class MessageConsumer : IConsumer
    {
        private readonly BrokerConnection _brokerConnection;

        public MessageConsumer(BrokerConnection brokerConnection)
        {
            _brokerConnection = brokerConnection;
        }

        public void Read(string queue)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string exchange)
        {
            throw new NotImplementedException();
        }
    }
}
