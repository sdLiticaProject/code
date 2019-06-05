using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public interface IMessage
    {
        Type Type { get; }        
        object Body { get; }
    }
}
