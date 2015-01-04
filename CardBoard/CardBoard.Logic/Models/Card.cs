using System;

namespace CardBoard.Models
{
    public class Card
    {
        private readonly Guid _cardId;

        public Card(Guid cardId)
        {
            _cardId = cardId;
        }

        public Guid CardId
        {
            get { return _cardId; }
        }

        public string Text { get; set; }
        public Column Column { get; set; }
    }
}
