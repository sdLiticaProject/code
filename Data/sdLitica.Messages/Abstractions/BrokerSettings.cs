using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public class BrokerSettings : IBrokerSettings
    {
        private readonly MessageSettings _messageSettings;

        public BrokerSettings(IAppSettings _AppSettings)
        {
            _messageSettings = _AppSettings.MessageSettings;
        }

        public string UserName => _messageSettings.UserName;

        public string Password => _messageSettings.Password;

        public string HostName => _messageSettings.Host;

        public string VirtualHost => _messageSettings.VirtualHost;

        public int Port => _messageSettings.Port;

        public string BuildUri()
        {
            throw new NotImplementedException();
        }
    }
}
