using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace sdLitica.Messages.Consumers
{
    /// <summary>
    /// Background class to consume a message
    /// </summary>
    internal class MessageConsumer : IConsumer
    {        
        private readonly IModel _channel;
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="brokerConnection"></param>
        public MessageConsumer(BrokerConnection brokerConnection)
        {
            _channel = brokerConnection?.CreateChannel()
                   ?? throw new ArgumentNullException(nameof(brokerConnection));
        }

        /// <summary>
        /// Read a message received through the bus
        /// </summary>
        /// <param name="queue"></param>
        public void Read(string queue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Subscribe a message received through the bus
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="action"></param>
        public void Subscribe(string exchange, string routingKey, Action<object> action)
        {
            _channel.ExchangeDeclare(exchange: exchange, type: "topic", durable: true);

            var queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (object model, BasicDeliverEventArgs ea) =>
            {
                var body = ea.Body;
                var strMessage = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<Message>(strMessage);
                if (message == null) throw new Exception("Could not deserialize message object");

                var eventAssembly = Assembly.Load("sdLitica.Events");
                var type = eventAssembly.GetType(message.Type);
                var instance = Activator.CreateInstance(type);

                var @event = JsonConvert.DeserializeObject(message.Body, type);

                action(@event);
            };

            _channel.BasicConsume(queue: queue,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
