using CardBoard.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Card : IMessageHandler
    {
        private static MessageDispatcher<Card> _dispatcher = new MessageDispatcher<Card>()
            .On("CardTextChanged", (o, m) => o.HandleCardTextChangedMessage(m))
            .On("CardMoved", (o, m) => o.HandleCardTextMovedMessage(m));

        private readonly Guid _cardId;

        private Mutable<string> _text = new Mutable<string>();
        private Mutable<Column> _column = new Mutable<Column>();

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

        public Message SetText(string text)
        {
            return _text.CreateMessage("CardTextChanged", _cardId, text);
        }

        public IEnumerable<Candidate<Column>> Column
        {
            get { return _column.Candidates; }
        }

        public Message MoveTo(Column column)
        {
            return _column.CreateMessage("CardMoved", _cardId, column);
        }

        public Guid GetObjectId()
        {
            return _cardId;
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        private void HandleCardTextChangedMessage(Message message)
        {
            _text.HandleMessage(message);
        }

        private void HandleCardTextMovedMessage(Message message)
        {
            _column.HandleMessage(message);
        }
    }
}
