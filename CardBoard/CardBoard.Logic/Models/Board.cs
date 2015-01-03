using Assisticant.Collections;
using Assisticant.Fields;
using CardBoard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Board
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

        public Message CreateCard()
        {
            var message = Message.CreateMessage(
                "CardCreated",
                new List<MessageHash>(),
                Guid.Empty,
                new { CardId = Guid.NewGuid() });
            return message;
        }

        public void DeleteCard(Card card)
        {
            throw new NotImplementedException();
        }

        public void HandleCardCreated(Message message)
        {
            Guid cardId = Guid.Parse(message.Body.CardId);
            if (!_cards.Any(c => c.CardId == cardId))
                _cards.Add(new Card(cardId));
        }
    }
}
