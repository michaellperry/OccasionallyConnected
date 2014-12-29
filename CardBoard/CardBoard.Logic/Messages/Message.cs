using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messages
{
    public class Message
    {
        public string Type { get; set; }
        public List<MessageHash> Predecessors { get; set; }
        public Guid ObjectId { get; set; }
        public JObject Body { get; set; }
        public MessageHash Hash { get; set; }

        public static Message CreateMessage(
            string messageType, 
            List<MessageHash> predecessors, 
            Guid objectId, 
            JObject body)
        {
            return new Message
            {
                Type = messageType,
                Predecessors = predecessors,
                ObjectId = objectId,
                Body = body,
                Hash = MessageHash.OfMessage(messageType, predecessors, body)
            };
        }
    }
}
