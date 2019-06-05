using RabbitMQ.Client;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Producers
{
    public class MessagePublisher : IPublisher
    {
        private readonly IModel _channel;

        public MessagePublisher(BrokerConnection brokerConnection)
        {
            _channel = brokerConnection?.CreateChannel() 
                ?? throw new ArgumentNullException(nameof(brokerConnection));
        }

        public void Publish(string exchange, IMessage message)
        {
            var message = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(exchange, "", body = body)
        }
    }
}
