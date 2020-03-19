using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    /// <summary>
    /// This base class provides initial data for events
    /// </summary>
    public abstract class Event : IEvent
    {
        /// <summary>
        /// Event Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event Datetime 
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Event Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Event()
        {
            Id = Guid.NewGuid();
            DateTime = DateTime.UtcNow;
            Name = GetType().FullName;
        }
    }
}
