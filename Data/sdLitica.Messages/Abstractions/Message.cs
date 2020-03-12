using System;
using System.Collections.Generic;
using System.Text;

namespace sdLitica.Messages.Abstractions
{
    public class Message : IMessage
    {
        public string Type { get; set; }

        public string Body { get; set; }

        public Message(string type, string serializedBody)
        {
            Type = type;
            Body = serializedBody;
        }
    }
}
