﻿using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace CardBoard.Messaging
{
    public class Message
    {
        private readonly string _type;
        private readonly ImmutableList<MessageHash> _predecessors;
        private readonly Guid _objectId;
        private readonly ExpandoObject _body;
        private readonly MessageHash _hash;

        private Message(
            string type,
            ImmutableList<MessageHash> predecessors,
            Guid objectId,
            ExpandoObject body,
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

        public MessageMemento GetMemento()
        {
            return new MessageMemento()
            {
                Hash = Hash.ToString(),
                MessageType = Type,
                Predecessors = Predecessors
                    .Select(p => p.ToString())
                    .ToList(),
                ObjectId = ObjectId,
                Body = Body
            };
        }

        public static Message CreateMessage(
            string messageType,
            IEnumerable<MessageHash> predecessors,
            Guid objectId,
            object body)
        {
            // Convert the anonymous typed object to an ExpandoObject.
            var expandoBody = JsonConvert.DeserializeObject<ExpandoObject>(
                JsonConvert.SerializeObject(body));
            object document = new
            {
                MessageType = messageType,
                Predecessors = predecessors
                    .Select(p => p.ToString())
                    .ToArray(),
                ObjectId = objectId,
                Body = expandoBody
            };
            var messageHash = new MessageHash(ComputeHash(document));

            return new Message(
                messageType,
                predecessors.ToImmutableList(),
                objectId,
                expandoBody,
                messageHash);
        }

        public static Message FromMemento(MessageMemento memento)
        {
            return new Message(
                memento.MessageType,
                memento.Predecessors
                    .Select(h => MessageHash.Parse(h))
                    .ToImmutableList(),
                memento.ObjectId,
                memento.Body,
                MessageHash.Parse(memento.Hash));
        }

        private static byte[] ComputeHash(object document)
        {
            var sha = new Sha256Digest();
            var stream = new DigestStream(new MemoryStream(), null, sha);
            using (var writer = new StreamWriter(stream))
            {
                string mementoToString = JsonConvert.SerializeObject(document);
                writer.Write(mementoToString);
            }
            byte[] buffer = new byte[sha.GetDigestSize()];
            sha.DoFinal(buffer, 0);
            return buffer;
        }
    }
}
