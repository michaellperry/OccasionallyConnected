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

            return Message.CreateMessage("CardCreated", new List<MessageHash>(), body);
        }
    }
}
