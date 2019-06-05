using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    public interface IEvent
    {
        Guid Id { get; }
        DateTime DateTime { get; }
        string Name { get; }
    }
}
