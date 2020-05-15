using Newtonsoft.Json;
using RabbitMQ.Client;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Producers
{
    /// <summary>
    /// Background class to publishes a message
    /// </summary>
    internal class MessagePublisher : IPublisher
    {
        private readonly IModel _channel;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="brokerConnection"></param>
        public MessagePublisher(BrokerConnection brokerConnection)
        {
            _channel = brokerConnection?.CreateChannel() 
                ?? throw new ArgumentNullException(nameof(brokerConnection));
        }

        /// <summary>
        /// Publish (direct) a message to the bus
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        public void Publish(string queue, IMessage message)
        {
            var serialized = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serialized);
            var properties = _channel.CreateBasicProperties();

            //if RabbitMQ restarts, the message will persist
            properties.Persistent = true;

            //TODO: support exchange publishing
            _channel.BasicPublish("", queue, properties, body);
        }

        /// <summary>
        /// Publish (topic) a message to the bus
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="message"></param>
        public void PublishToTopic(string exchange, string routingKey, IMessage message)
        {
            var serialized = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(serialized);
            var properties = _channel.CreateBasicProperties();

            //if RabbitMQ restarts, the message will persist
            properties.Persistent = true;

            _channel.BasicPublish(exchange, routingKey, properties, body);
        }
    }
}