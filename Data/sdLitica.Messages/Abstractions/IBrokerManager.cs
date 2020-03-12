using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public interface IBrokerManager
    {
        void CreateExchange(string name);
        void CreateQueue(string name);        
    }
}
