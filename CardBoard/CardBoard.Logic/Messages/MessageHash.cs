using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using System.IO;

namespace CardBoard.Messages
{
    public class MessageHash
    {
        private byte[] _value;

        private MessageHash(byte[] value)
        {
            _value = value;
        }

        public byte[] Value
        {
            get { return _value; }
        }

        public static MessageHash OfMessage(string messageType, IEnumerable<MessageHash> predecessors, JObject body)
        {
            JObject memento = new JObject(
                new JProperty("MessageType", messageType),
                new JProperty("Predecessors", new JArray(
                    from p in predecessors
                    select Convert.ToBase64String(p.Value))),
                new JProperty("Body", body));
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(memento.ToString());
            }
            byte[] buffer = new byte[sha.GetByteLength()];
            sha.DoFinal(buffer, 0);
            return new MessageHash(buffer);
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
