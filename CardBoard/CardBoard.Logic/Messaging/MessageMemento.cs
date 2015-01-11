using System;
using System.Collections.Generic;
using System.Dynamic;

namespace CardBoard.Messaging
{
    public class MessageMemento
    {
        public string Hash { get; set; }
        public string MessageType { get; set; }
        public List<string> Predecessors { get; set; }
        public Guid ObjectId { get; set; }
        public ExpandoObject Body { get; set; }
    }
}
