using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    public abstract class Event : IEvent
    {
        public Guid Id { get; set; }

        public DateTime DateTime { get; set; }

        public string Name { get; set; }

        public Event()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
            Name = GetType().FullName;
        }
    }
}
