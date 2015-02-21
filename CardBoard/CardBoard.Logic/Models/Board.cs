using Assisticant.Collections;
using Assisticant.Fields;
using CardBoard.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace CardBoard.Models
{
    public class Board : IMessageHandler
    {

        private static readonly MessageDispatcher<Board> _dispatcher = new MessageDispatcher<Board>()
            .On("CardCreated", (o, m) => o.HandleCardCreatedMessage(m))
            .On("CardDeleted", (o, m) => o.HandleCardDeletedMessage(m));

        private Observable<string> _name = new Observable<string>("Pluralsight");
        private Observable<ImmutableList<Card>> _cards =
            new Observable<ImmutableList<Card>>(ImmutableList<Card>.Empty);

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            var cardsDeleted = messages
                .Where(m => m.Type == "CardDeleted")
                .Select(m => Guid.Parse(m.Body.CardId))
                .ToLookup(g => g);
            var cardsCreated = messages
                .Where(m => m.Type == "CardCreated")
                .Select(m => Guid.Parse(m.Body.CardId))
                .Where(g => !cardsDeleted.Contains(g))
                .Distinct()
                .Select(g => new Card(g, GetObjectId().ToCanonicalString()));

            lock (this)
            {
                _cards.Value = _cards.Value.AddRange(cardsCreated);
            }
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<Card> Cards
        {
            get { return _cards.Value; }
        }

        public Message CreateCard(Guid cardId)
        {
            return Message.CreateMessage(
                GetObjectId().ToCanonicalString(),
                "CardCreated",
                Guid.Empty,
                new
                {
                    CardId = cardId
                });
        }

        public Message DeleteCard(Guid cardId)
        {
            return Message.CreateMessage(
                GetObjectId().ToCanonicalString(),
                "CardDeleted",
                Guid.Empty,
                new
                {
                    CardId = cardId
                });
        }

        private void HandleCardCreatedMessage(Message message)
        {
            Guid cardId = Guid.Parse(message.Body.CardId);
            lock (this)
            {
                var cards = _cards.Value;
                if (!cards.Any(c => c.CardId == cardId))
                    _cards.Value = cards.Add(new Card(cardId, GetObjectId().ToCanonicalString()));
            }
        }

        private void HandleCardDeletedMessage(Message message)
        {
            Guid cardId = Guid.Parse(message.Body.CardId);
            lock (this)
            {
                _cards.Value = _cards.Value.RemoveAll(c => c.CardId == cardId);
            }
        }

        public Guid GetObjectId()
        {
            return Guid.Empty;
        }
    }
}
