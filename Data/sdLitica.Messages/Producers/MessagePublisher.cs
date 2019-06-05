using sdLitica.Messages.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Producers
{
    public class MessagePublisher : IPublisher
    {        
        public void Publish(string queue, IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
