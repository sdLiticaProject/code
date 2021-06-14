using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Extensions;
using sdLitica.Messages.Abstractions;

namespace sdLitica.Events.Rpc
{
    /// <summary>
    /// A handy static-method utility to register necessary 'IEvent handler(IEvent)' methods
    /// as an rpc server endpoints which will be received and processed over RabbitMQ broker
    /// <see href="https://www.rabbitmq.com/tutorials/tutorial-six-dotnet.html"/>
    /// </summary>
    public static class RpcServerRabbitMq
    {
        public static void RegisterHandlers(IServiceProvider services, IDictionary<Type, Func<IEvent, IEvent>> handlers)
        {
            var registry = services.GetRequiredService<IEventRegistry>();
            var connection = services.GetRequiredService<BrokerConnection>();
            var channel = connection.CreateChannel();
            foreach (var (type, handler) in handlers)
            {
                var exchange = registry.GetPublishingTarget(type).Single();
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