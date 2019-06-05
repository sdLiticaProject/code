using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    public abstract class Event : IEvent
    {
        public Guid Id { get; private set; }

        public DateTime DateTime { get; private set; }

        public string Name { get; private set; }

        public Event()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
            Name = GetType().FullName;
        }
    }
}
