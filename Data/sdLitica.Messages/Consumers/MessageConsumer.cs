using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Consumers
{
    public class MessageConsumer : IConsumer
    {        
        private readonly IModel _channel;
        
        public MessageConsumer(BrokerConnection brokerConnection)
        {
            _channel = brokerConnection?.CreateChannel()
                   ?? throw new ArgumentNullException(nameof(brokerConnection));
        }

        public void Read(string queue)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string exchange, Action<object> action)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (object model, BasicDeliverEventArgs ea) =>
            {
                var body = ea.Body;
                var strMessage = Encoding.UTF8.GetString(body);

                var message = JsonConvert.DeserializeObject<Message>(strMessage);
                if (message == null) throw new Exception("Could not deserialize message object");
                
                var type = Type.GetType(message.Type);
                var instance = Activator.CreateInstance(type);

                var @event = JsonConvert.DeserializeObject(message.Body, type);

                action(@event);
            };

            _channel.BasicConsume(queue: exchange,
                                 autoAck: true,
                                 consumer: consumer);
        }
    }
}
