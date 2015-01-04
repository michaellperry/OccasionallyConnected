using Newtonsoft.Json;
using System;
using System.Dynamic;

namespace CardBoard.Messaging
{
    public class Message
    {
        private readonly string _type;
        private readonly Guid _objectId;
        private readonly ExpandoObject _body;

        private Message(
            string type,
            Guid objectId,
            ExpandoObject body)
        {
            _type = type;
            _objectId = objectId;
            _body = body;
        }

        public string Type
        {
            get { return _type; }
        }

        public Guid ObjectId
        {
            get { return _objectId; }
        }

        public dynamic Body
        {
            get { return _body; }
        }

        public static Message CreateMessage(
            string messageType,
            Guid objectId,
            object body)
        {
            // Convert the anonymous typed object to an ExpandoObject.
            var expandoBody = JsonConvert.DeserializeObject<ExpandoObject>(
                JsonConvert.SerializeObject(body));

            return new Message(
                messageType,
                objectId,
                expandoBody);
        }
    }
}
