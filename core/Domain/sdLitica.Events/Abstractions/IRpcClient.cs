namespace sdLitica.Events.Abstractions
{
    /// <summary>
    /// A handy single-method interface which supposed to be
    /// a wrapper for rpc client to execute unary calls over RabbitMQ broker,
    /// but can be abstracted for rpc communication over grpc/whatever protocol
    /// See interface implementations for details
    /// </summary>
    public interface IRpcClient
    {
        /// <summary>
        /// Call method supposed to send an IEvent @event parameter to RabbitMQ,
        /// wait for a response and return it as an another instance of IEvent type
        /// </summary>
        /// <param name="event">A rpc request which should be published to RabbitMQ</param>
        /// <returns>A response of IEvent type which should be received from RabbitMQ</returns>
        IEvent Call(IEvent @event);
    }
}