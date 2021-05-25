using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Events.Abstractions
{
    /// <summary>
    /// This interface provides base data structure for events
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Event Id
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Datetime of event creation
        /// </summary>
        DateTime DateTime { get; }
        /// <summary>
        /// Name of the event
        /// </summary>
        string Name { get; }
    }
}
