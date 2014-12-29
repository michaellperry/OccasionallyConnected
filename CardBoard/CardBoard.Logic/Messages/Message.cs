using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CardBoard.Messages
{
    public class Message
    {
        private readonly string _type;
        private readonly List<MessageHash> _predecessors;
        private readonly Guid _objectId;
        private readonly JObject _body;
        private readonly MessageHash _hash;

        private Message(
            string type,
            List<MessageHash> predecessors,
            Guid objectId,
            JObject body,
            MessageHash hash)
        {
            _type = type;
            _predecessors = predecessors;
            _objectId = objectId;
            _body = body;
            _hash = hash;
        }

        public string Type
        {
            get { return _type; }
        }

        public IEnumerable<MessageHash> Predecessors
        {
            get { return _predecessors; }
        }

        public Guid ObjectId
        {
            get { return _objectId; }
        }

        public JObject Body
        {
            get { return _body; }
        }

        public MessageHash Hash
        {
            get { return _hash; }
        }

        public static Message CreateMessage(
            string messageType,
            IEnumerable<MessageHash> predecessors,
            Guid objectId,
            JObject body)
        {
            return new Message(
                messageType,
                predecessors.ToList(),
                objectId,
                body,
                ComputeHash(messageType, predecessors, objectId, body));
        }

        private static MessageHash ComputeHash(
            string messageType,
            IEnumerable<MessageHash> predecessors,
            Guid objectId,
            JObject body)
        {
            JObject memento = new JObject(
                new JProperty("MessageType", messageType),
                new JProperty("Predecessors", new JArray(
                    from p in predecessors
                    select Convert.ToBase64String(p.Value))),
                new JProperty("ObjectId", objectId),
                new JProperty("Body", body));
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                string mementoToString = memento.ToString();
                writer.Write(mementoToString);
            }
            byte[] buffer = new byte[sha.GetByteLength()];
            sha.DoFinal(buffer, 0);
            return new MessageHash(buffer);
        }
    }
}
