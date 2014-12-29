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

        private MessageHash(byte[] value)
        {
            _value = value;
        }

        public static MessageHash OfMessage(string messageType, IEnumerable<MessageHash> predecessors, JObject body)
        {
            return new MessageHash(new byte[] { 0, 0, 0, 0 });
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (MessageHash)obj;
            return Enumerable.SequenceEqual(this._value, that._value);
        }

        public override int GetHashCode()
        {
            int hash = 0;
            foreach (var b in _value)
                hash = hash * 37 + b;

            return hash;
        }
    }
}
