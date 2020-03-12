using Newtonsoft.Json;
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

        public void Publish(string queue, IMessage message)
        {
            var serialized = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serialized);
            var properties = _channel.CreateBasicProperties();
            
            //if RabbitMQ restarts, the message will persist
            properties.Persistent = true;
            
            _channel.BasicPublish("", queue, properties, body);
        }
    }
}