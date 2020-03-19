using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// Interface to broker settings
    /// </summary>
    public interface IBrokerSettings
    {
        /// <summary>
        /// Broker Username
        /// </summary>
        string UserName { get; }
        /// <summary>
        /// Broker Password
        /// </summary>
        string Password { get; }
        /// <summary>
        /// Broker HostName
        /// </summary>
        string HostName { get; }
        /// <summary>
        /// Broker Username
        /// </summary>
        string VirtualHost { get; }
        /// <summary>
        /// Broker Port
        /// </summary>
        int Port { get; }
        /// <summary>
        /// Build entire URL to broker connection
        /// </summary>
        string BuildUri();
    }
}
