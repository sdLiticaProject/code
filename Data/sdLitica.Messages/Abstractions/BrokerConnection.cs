using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    internal class BrokerConnection : IDisposable
    {
        private readonly IBrokerSettings _settings;
        private IConnection _connection;
        private IModel _model;

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
                VirtualHost = _settings.VirtualHost
            };

            _connection = factory.CreateConnection();
            _model = _connection.CreateModel();            
        }

        public void CreateQueue()
        {
            CreateConnection();            
        }

        public void CreateExchange()
        {
            CreateConnection();
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
