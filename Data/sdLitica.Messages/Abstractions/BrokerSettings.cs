using sdLitica.Utils.Abstractions;
using sdLitica.Utils.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// This class defines broker settings
    /// </summary>
    internal class BrokerSettings : IBrokerSettings
    {
        private readonly MessageSettings _messageSettings;

        /// <summary>
        /// Default cosntructor
        /// </summary>
        /// <param name="_AppSettings"></param>
        public BrokerSettings(IAppSettings _AppSettings)
        {
            _messageSettings = _AppSettings.MessageSettings;
        }

        /// <summary>
        /// Username
        /// </summary>
        public string UserName => _messageSettings.UserName;
        /// <summary>
        /// Passwork
        /// </summary>
        public string Password => _messageSettings.Password;
        /// <summary>
        /// Hostname
        /// </summary>
        public string HostName => _messageSettings.Host;
        /// <summary>
        /// VirtualHost
        /// </summary>
        public string VirtualHost => _messageSettings.VirtualHost;
        /// <summary>
        /// Port
        /// </summary>
        public int Port => _messageSettings.Port;

        /// <summary>
        /// Build entire URL to broker connection
        /// </summary>
        /// <returns></returns>
        public string BuildUri()
        {
            throw new NotImplementedException();
        }
    }
}
