using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// Class to deal with broker connection
    /// </summary>
    internal class BrokerConnection : IDisposable
    {
        private readonly IBrokerSettings _settings;
        private IConnection _connection;
        private IModel _model;
        private object _lock = new object();

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings"></param>
        public BrokerConnection(IBrokerSettings settings)
        {
            _settings = settings;
        }

        /// <summary>
        /// Create broker connection
        /// </summary>
        public void CreateConnection()
        {            
            if (_connection != null && _connection.IsOpen) return;
            if (_settings == null) throw new ArgumentNullException(nameof(_settings));

            var factory = new ConnectionFactory()
            {
                UserName = _settings.UserName,
                Password = _settings.Password,
                HostName = _settings.HostName,
                VirtualHost = _settings.VirtualHost,
                Port = _settings.Port
            };

            _connection = factory.CreateConnection();               
        }

        /// <summary>
        /// Create queue in the broker
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="durable"></param>
        /// <param name="exclusive"></param>
        /// <param name="autoDelete"></param>
        /// <param name="arguments"></param>
        public void CreateQueue(string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            EnsureModelCreate();

            _model.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }
     
        /// <summary>
        /// Create exchange in the broker
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="type"></param>
        /// <param name="durable"></param>
        /// <param name="autoDelete"></param>
        /// <param name="arguments"></param>
        public void CreateExchange(string exchange, string type, bool durable = true, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            EnsureModelCreate();
            
            _model.ExchangeDeclare(exchange, type, durable, autoDelete, arguments);            
        }

       
        private void EnsureModelCreate()
        {
            lock (_lock)
            {
                if (_model == null)
                    _model = CreateChannel();
            }
        }

        /// <summary>
        /// Create channel to send and receive messages
        /// </summary>
        /// <returns></returns>
        public IModel CreateChannel()
        {
            CreateConnection();
            return _connection.CreateModel();
        }


        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _model?.Close();
            _model?.Dispose();
            _connection?.Close();            
            _connection?.Dispose();
        }
    }
}