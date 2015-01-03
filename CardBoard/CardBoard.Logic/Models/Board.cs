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

        public Card NewCard()
        {
            Guid cardId = Guid.NewGuid();
            var message = MessageFactory.CardCreated(cardId);
            HandleCardCreated(message);
            return _cards.Where(c => c.CardId == cardId).Single();
        }

        public void DeleteCard(Card card)
        {
            throw new NotImplementedException();
        }

        public void HandleCardCreated(Message message)
        {
            var cardId = message.Body.CardId;
            if (!_cards.Any(c => c.CardId == cardId))
                _cards.Add(new Card(cardId));
        }
    }
}
