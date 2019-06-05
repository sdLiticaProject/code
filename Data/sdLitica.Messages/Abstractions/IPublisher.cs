using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public interface IPublisher
    {
        void Publish(string queue, IMessage message);
    }
}
