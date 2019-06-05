using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public interface IBrokerSettings
    {
        string UserName { get; }
        string Password { get; }
        string HostName { get; }      
        string VirtualHost { get; }
        int Port { get; }

        string BuildUri();
    }
}
