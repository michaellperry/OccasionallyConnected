using Assisticant.Collections;
using Assisticant.Fields;
using System.Collections.Generic;
using CardBoard.Messaging;
using System;
using System.Linq;

namespace CardBoard.Models
{
    public class Board
    {
        private Observable<string> _name = new Observable<string>("Pluralsight");
        private ObservableList<Card> _cards = new ObservableList<Card>();

        public void HandleMessage(Message message)
        {
            if (message.Type == "CardCreated")
                HandleCardCreatedMessage(message);
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        private void HandleCardCreatedMessage(Message message)
        {
            Guid cardId = Guid.Parse(message.Body.CardId);
            if (!_cards.Any(c => c.CardId == cardId))
                _cards.Add(new Card(cardId));
        }
    }
}
