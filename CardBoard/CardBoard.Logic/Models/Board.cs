using Assisticant.Collections;
using CardBoard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Board
    {
        private ObservableList<Card> _cards = new ObservableList<Card>();

        public IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        public void HandleCardCreated(Message message)
        {
            var cardId = message.Body.CardId;
            if (!_cards.Any(c => c.CardId == cardId))
                _cards.Add(new Card(cardId));
        }
    }
}
