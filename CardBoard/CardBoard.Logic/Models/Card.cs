using CardBoard.Messaging;
using System;
using System.Collections.Generic;

namespace CardBoard.Models
{
    public class Card : IMessageHandler
    {
        private static MessageDispatcher<Card> _dispatcher = new MessageDispatcher<Card>()
            .On("CardTextChanged", (o, m) => HandleCardTextChangedMessage(m))
            .On("CardMoved", (o, m) => HandleCardTextMovedMessage(m));
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

        public Column Column { get; set; }

        public Guid GetObjectId()
        {
            return _cardId;
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        private static void HandleCardTextChangedMessage(Message message)
        {
            throw new NotImplementedException();
        }

        private static void HandleCardTextMovedMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
