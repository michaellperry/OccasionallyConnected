using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.IO;
using System;
using System.Collections.Generic;
using System.IO;
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
            IEnumerable<MessageHash> predecessors,
            Guid objectId,
            JObject body)
        {
            return new Message
            {
                Type = messageType,
                Predecessors = predecessors.ToList(),
                ObjectId = objectId,
                Body = body,
                Hash = ComputeHash(messageType, predecessors, objectId, body)
            };
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
