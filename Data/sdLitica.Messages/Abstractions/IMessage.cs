using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public interface IMessage
    {
        string Type { get; }        
        string Body { get; }
    }
}
