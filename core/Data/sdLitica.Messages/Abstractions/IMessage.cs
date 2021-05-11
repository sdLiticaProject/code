using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// This interface represents a message.
    /// IMessage is the interface that defines serialization to be send through the bus.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Event type (e.g. sdLitica.Events.Integration.TimeSeriesAnalysisEvent)
        /// </summary>
        string Type { get; }
        /// <summary>
        /// Event serialized that comprise the event data itselfs
        /// </summary>
        string Body { get; }
    }
}
