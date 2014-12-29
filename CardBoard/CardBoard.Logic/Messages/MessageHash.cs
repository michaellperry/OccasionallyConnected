using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CardBoard.Messages
{
    public class MessageHash
    {
        private byte[] _value;

        public static MessageHash OfMessage(string messageType, IEnumerable<MessageHash> predecessors, JObject body)
        {
            return new MessageHash();
        }
    }
}
