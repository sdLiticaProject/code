using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public class BrokerConnection : IDisposable
    {
        private readonly IBrokerSettings _settings;
        private IConnection _connection;
        private IModel _model;
        private object _lock = new object();
        public BrokerConnection(IBrokerSettings settings)
        {
            _settings = settings;
        }

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

        public void CreateQueue(string queue, bool durable = true, bool exclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            EnsureModelCreate();

            _model.QueueDeclare(queue, durable, exclusive, autoDelete, arguments);
        }
     

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

        public IModel CreateChannel()
        {
            CreateConnection();
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            _model?.Close();
            _model?.Dispose();
            _connection?.Close();            
            _connection?.Dispose();
        }
    }
}