using CardBoard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Card
    {
        private readonly Guid _cardId;

        private Mutable<string> _text = new Mutable<string>();

        public Card(Guid cardId)
        {
            _cardId = cardId;
        }

        public Guid CardId
        {
            get { return _cardId; }
        }

        public IEnumerable<Candidate<string>> Text
        {
            get { return _text.Candidates; }
        }

        public List<Candidate<Column>> Column { get; set; }

        public void SetCardText(MessageHash messageHash, string value, IEnumerable<MessageHash> predecessors)
        {
            _text.SetValue(messageHash, value, predecessors);
        }
    }
}
