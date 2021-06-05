using System;
using System.Collections.Generic;
using System.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Extensions;

namespace sdLitica.Events.Rpc
{
    /// <summary>
    /// A handy static-method utility to register necessary 'IEvent handler(IEvent)' methods
    /// as an rpc server endpoints which will be received and processed over RabbitMQ broker
    /// <see href="https://www.rabbitmq.com/tutorials/tutorial-six-dotnet.html"/>
    /// </summary>
    public static class RpcServerRabbitMq
    {
        public static void RegisterHandlers(IEventRegistry eventRegistry, IModel channel, IDictionary<IEvent, Func<IEvent, IEvent>> handlers)
        {
            foreach (var (@event, handler) in handlers)
            {
                var exchange = eventRegistry.GetPublishingTarget(@event.GetType()).Single();
                var queue = Exchanges.GetRpcQueue(exchange);
                channel.QueueDeclare(queue);
                var consumer = new EventingBasicConsumer(channel);
                channel.BasicConsume(queue, false, consumer);
                consumer.Received += (_, ea) =>
                {
                    var request = ea.Body.ToArray().ToEvent();
                    var response = handler(request);

                    var bytes = response.ToBytes();
                    var props = ea.BasicProperties;
                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;
                    channel.BasicPublish(exchange, props.ReplyTo, replyProps, bytes);
                    channel.BasicAck(ea.DeliveryTag, false);
                };
            }
        }
    }
}