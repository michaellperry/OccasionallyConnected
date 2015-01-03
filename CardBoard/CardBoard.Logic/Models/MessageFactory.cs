using CardBoard.Messages;
using System;
using System.Collections.Generic;

namespace CardBoard.Models
{
    public static class MessageFactory
    {
        public static Message CardTextChanged(Guid cardId, string value, IEnumerable<MessageHash> predecessors)
        {
            return Message.CreateMessage(
                "CardTextChanged",
                predecessors,
                cardId,
                new { Value = value });
        }

        public static Message CardMoved(Guid cardId, Column value, IEnumerable<MessageHash> predecessors)
        {
            return Message.CreateMessage(
                "CardMoved",
                predecessors,
                cardId,
                new { Value = value });
        }
    }
}
