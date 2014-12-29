using CardBoard.Messages;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Models
{
    public static class MessageFactory
    {
        public static Message CardCreated(Guid cardId)
        {
            JObject body = new JObject();
            body["CardId"] = cardId;

            return Message.CreateMessage("CardCreated", new List<MessageHash>(), Guid.Empty, body);
        }

        public static Message CardTextChanged(Guid cardId, string value)
        {
            JObject body = new JObject();
            body["Value"] = value;

            return Message.CreateMessage("CardTextChanged", new List<MessageHash>(), cardId, body);
        }
    }
}
