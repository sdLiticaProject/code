﻿using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Messages.Abstractions;

namespace sdLitica.Messages.Consumers
{
    /// <summary>
    /// Background class to consume a message
    /// </summary>
    internal class MessageConsumer: IConsumer
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
        /// Subscribe (topic) a message received through the bus
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="routingKey"></param>
        /// <param name="action"></param>
        public void SubscribeToTopic(string exchange, string routingKey, Action<object> action)
        {
            string queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue, exchange, routingKey);

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, ea) =>
            {
                object @event = CreateEventFromMessage(ea);
                action(@event);
            };

            _channel.BasicConsume(queue, true, consumer);
        }

        /// <summary>
        /// Subscribe (direct) a message received through the bus
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="action"></param>
        public void Subscribe(string exchange, Action<object> action)
        {
            string queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue, exchange, "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (_, ea) =>
            {
                object @event = CreateEventFromMessage(ea);
                action(@event);
            };

            _channel.BasicConsume(queue, true, consumer);
        }

        /// <summary>
        /// Subscribe (direct) a message received through the bus
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="action"></param>
        public void Subscribe(string exchange, Func<object, Task> action)
        {
            string queue = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue, exchange, "");

            EventingBasicConsumer consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (_, ea) =>
            {
                object @event = CreateEventFromMessage(ea);
                await action(@event);
            };

            _channel.BasicConsume(queue, true, consumer);
        }

        private static object CreateEventFromMessage(BasicDeliverEventArgs ea)
        {
            byte[] body = ea.Body.ToArray();
            string strMessage = Encoding.UTF8.GetString(body);

            Message message = JsonConvert.DeserializeObject<Message>(strMessage)
                              ?? throw new Exception("Could not deserialize message object");

            Assembly eventAssembly = Assembly.Load("sdLitica.Events");
            Type type = eventAssembly.GetType(message.Type)
                        ?? throw new Exception("Could not find corresponding type for event");
            return JsonConvert.DeserializeObject(message.Body, type);
        }
    }
}