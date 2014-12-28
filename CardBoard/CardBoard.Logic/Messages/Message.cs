using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messages
{
    public class Message
    {
        public MessageHash Hash { get; set; }
        public List<MessageHash> Predecessors { get; set; }
        public string Type { get; set; }
        public JObject Body { get; set; }

        public static Message CreateMessage(string messageType, List<MessageHash> predecessors, JObject body)
        {
            return new Message
            {
                Type = messageType,
                Predecessors = predecessors,
                Body = body,
                Hash = MessageHash.OfMessage(messageType, predecessors, body)
            };
        }
    }
}
