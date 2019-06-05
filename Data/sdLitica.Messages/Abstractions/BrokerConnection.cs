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
        }

        public void CreateQueue()
        {
            EnsureModelCreate();

            _model.QueueDeclare("FakeQueue", true, true, false);            
        }
     

        public void CreateExchange()
        {
            EnsureModelCreate();

            _model.ExchangeDeclare("FakeExchange", ExchangeType.Direct, true, true);            
        }

        private void EnsureModelCreate()
        {
            if (_model == null)
                _model = CreateChannel();
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