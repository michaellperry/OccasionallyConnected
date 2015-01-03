using Assisticant.Collections;
using Assisticant.Fields;
using CardBoard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Board : IMessageHandler
    {
        private Observable<string> _name = new Observable<string>("Pluralsight");
        private ObservableList<Card> _cards = new ObservableList<Card>();

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        public Message CreateCard(Guid cardId)
        {
            return Message.CreateMessage(
                "CardCreated",
                new List<MessageHash>(),
                Guid.Empty,
                new { CardId = cardId });
        }

        public Message DeleteCard(Card card)
        {
            return Message.CreateMessage(
                "CardDeleted",
                new List<MessageHash>(),
                Guid.Empty,
                new { CardId = card.CardId });
        }

        public void HandleCardCreated(Message message)
        {
            Guid cardId = Guid.Parse(message.Body.CardId);
            if (!_cards.Any(c => c.CardId == cardId))
                _cards.Add(new Card(cardId));
        }

        public Guid GetObjectId()
        {
            return Guid.Empty;
        }

        public IEnumerable<IMessageHandler> GetChildMessageHandlers()
        {
            return _cards;
        }

        public void HandleMessage(Message message)
        {
            if (message.Type == "CardCreated")
                HandleCardCreated(message);
        }
    }
}
