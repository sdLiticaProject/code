using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using sdLitica.Events.Abstractions;
using sdLitica.Events.Bus;
using sdLitica.Events.Extensions;
using sdLitica.Messages.Abstractions;

namespace sdLitica.Events.Rpc
{
    /// <summary>
    /// A handy single-method interface which supposed to be
    /// a wrapper for rpc client to execute unary calls over RabbitMQ broker
    /// It's an blocking implementation of unary rpc call
    /// and can be improved for async request execution,
    /// and/or can be abstracted for rpc communication over grpc/whatever protocol
    /// <see href="https://www.rabbitmq.com/tutorials/tutorial-six-dotnet.html"/>
    /// </summary>
    internal class RpcClientRabbitMq: IRpcClient
    {
        private readonly IModel _channel;
        private readonly IEventRegistry _eventRegistry;
        private readonly EventingBasicConsumer _consumer;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<IEvent>> _callbackMapper = new();

        public RpcClientRabbitMq(BrokerConnection brokerConnection, IEventRegistry eventRegistry)
        {
            _channel = brokerConnection?.CreateChannel()
                       ?? throw new ArgumentNullException(nameof(brokerConnection));
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (_, ea) =>
            {
                if (!_callbackMapper.TryRemove(ea.BasicProperties.CorrelationId, out TaskCompletionSource<IEvent> tcs))
                    return;
                var response = ea.Body.ToArray().ToEvent();
                tcs.TrySetResult(response);
            };
            _eventRegistry = eventRegistry;
        }

        public IEvent Call(IEvent @event)
        {
            var exchange = _eventRegistry.GetPublishingTarget(@event.GetType()).Single();
            var queue = Exchanges.GetRpcQueue(exchange);
            // A queue supposed to be 'declared' by corresponding rpc server
            _channel.QueueBind(queue, exchange, "");

            var props = _channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.Persistent = true;
            props.CorrelationId = correlationId;
            props.ReplyTo = _channel.QueueDeclare().QueueName;
            var tcs = new TaskCompletionSource<IEvent>();
            _callbackMapper.TryAdd(correlationId, tcs);

            var body = @event.ToBytes();
            _channel.BasicPublish(exchange, queue, props, body);
            _channel.BasicConsume(props.ReplyTo, true, _consumer);

            return tcs.Task.Result;
        }
    }
}