using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public interface IConsumer
    {
        void Read(string queue);
        void Subscribe(string exchange, Action<object> action);
    }
}
