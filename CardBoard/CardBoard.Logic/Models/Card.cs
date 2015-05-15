using Assisticant.Fields;
using CardBoard.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CardBoard.Models
{
    public class Card : IMessageHandler
    {
        #region Ignore this

        private static MessageDispatcher<Card> _dispatcher = new MessageDispatcher<Card>()
            .On("CardTextChanged", (o, m) => o.HandleCardTextChangedMessage(m))
            .On("CardMoved", (o, m) => o.HandleCardMovedMessage(m));

        private readonly Guid _cardId;

        private Mutable<string> _text;

        public Card(Guid cardId, string topic)
        {
            _cardId = cardId;
            _text = new Mutable<string>(topic);
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

        public Guid GetObjectId()
        {
            return _cardId;
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _text.HandleAllMessages(messages
                .Where(m => m.Type == "CardTextChanged"));
        }

        private void HandleCardTextChangedMessage(Message message)
        {
            _text.HandleMessage(message);
        }

        #endregion

        private HashSet<MessageHash> _predecessors =
            new HashSet<MessageHash>();
        private List<Candidate<Column>> _candidates =
            new List<Candidate<Column>>();

        public IEnumerable<Candidate<Column>> Column
        {
            get { return _candidates; }
        }

        public Message MoveTo(Column column)
        {
            return Message.CreateMessage(
                "topic",
                "CardMoved",
                _candidates.Select(t => t.MessageHash),
                _cardId,
                new
                {
                    Value = column
                });
        }

        private void HandleCardMovedMessage(Message message)
        {
        }
    }
}
