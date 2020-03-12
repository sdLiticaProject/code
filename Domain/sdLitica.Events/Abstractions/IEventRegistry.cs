using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    public interface IEventRegistry
    {
        void Register<T>(string exchange) where T : IEvent;
        IList<string> GetExchangesForEvent<T>(T @event) where T : IEvent;        
    }
}
