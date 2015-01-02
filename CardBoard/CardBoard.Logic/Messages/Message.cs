using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace CardBoard.Messages
{
    public class Message
    {
        private readonly string _type;
        private readonly ImmutableList<MessageHash> _predecessors;
        private readonly Guid _objectId;
        private readonly dynamic _body;
        private readonly MessageHash _hash;

        private Message(
            string type,
            ImmutableList<MessageHash> predecessors,
            Guid objectId,
            dynamic body,
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

        public IImmutableList<MessageHash> Predecessors
        {
            get { return _predecessors; }
        }

        public Guid ObjectId
        {
            get { return _objectId; }
        }

        public dynamic Body
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
            dynamic body)
        {
            return new Message(
                messageType,
                predecessors.ToImmutableList(),
                objectId,
                body,
                ComputeHash(messageType, predecessors, objectId, body));
        }

        private static MessageHash ComputeHash(
            string messageType,
            IEnumerable<MessageHash> predecessors,
            Guid objectId,
            dynamic body)
        {
            dynamic memento = new
            {
                MessageType = messageType,
                Predecessors = predecessors
                    .Select(p => Convert.ToBase64String(p.Value))
                    .ToArray(),
                ObjectId = objectId,
                Body = body
            };
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                string mementoToString = JsonConvert.SerializeObject(memento);
                writer.Write(mementoToString);
            }
            byte[] buffer = new byte[sha.GetByteLength()];
            sha.DoFinal(buffer, 0);
            return new MessageHash(buffer);
        }
    }
}
