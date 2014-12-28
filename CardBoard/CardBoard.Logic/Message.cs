using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Logic
{
    public class Message
    {
        public MessageHash Hash { get; set; }
        public List<MessageHash> Predecessors { get; set; }
        public string Type { get; set; }
        public JsonDocument Body { get; set; }
    }
}
