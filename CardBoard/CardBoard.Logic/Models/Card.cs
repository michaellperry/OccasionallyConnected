using CardBoard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Card
    {
        private readonly Guid _cardId;

        private List<Candidate<string>> _text = new List<Candidate<string>>();

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
            get { return _text; }
        }

        public List<Candidate<Column>> Column { get; set; }

        public void SetCardText(MessageHash messageHash, string value, IEnumerable<MessageHash> predecessors)
        {
            _text.RemoveAll(c => predecessors.Contains(c.MessageHash));
            _text.Add(new Candidate<string>(messageHash, value));
        }
    }
}
