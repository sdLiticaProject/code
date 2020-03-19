using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    /// <summary>
    /// This class represents a Message. 
    /// Message is the class that is serialized to be send through the bus.
    /// </summary>
    public class Message : IMessage
    {
        /// <summary>
        /// Event type (e.g. sdLitica.Events.Integration.TimeSeriesAnalysisEvent)
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Event serialized that comprise the event data itselfs
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Default cosntructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializedBody"></param>
        public Message(string type, string serializedBody)
        {
            Type = type;
            Body = serializedBody;
        }
    }
}
