using CardBoard.Messaging;
using CardBoard.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;

namespace CardBoard.Test
{
    [TestClass]
    public class GivenOneCard
    {
        private Application _application;
        private Card _card;

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application(new MemoryMessageQueue());
            var cardId = Guid.NewGuid();
            _application.EmitMessage(_application.Board.CreateCard(cardId));
            _card = _application.Board.Cards.Single(c => c.CardId == cardId);
        }

        [TestMethod]
        public void CardTextChanged()
        {
            _application.EmitMessage(CardTextChanged("Initial Text"));

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Initial Text");
        }

        [TestMethod]
        public void CardTextChangedAgain()
        {
            var firstMessage = CardTextChanged("Initial Text");
            var successor = CardTextChanged("New Text", firstMessage.Hash);

            _application.EmitMessage(firstMessage);
            _application.EmitMessage(successor);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void MessagesReceivedOutOfOrder()
        {
            var firstMessage = CardTextChanged("Initial Text");
            var successor = CardTextChanged("New Text", firstMessage.Hash);

            _application.EmitMessage(successor);
            _application.EmitMessage(firstMessage);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void CardMoved()
        {
            _application.EmitMessage(CardMoved(Column.ToDo));

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.ToDo);
        }

        [TestMethod]
        public void CardMovedAgain()
        {
            Message firstMessage = CardMoved(Column.ToDo);
            Message successor = CardMoved(Column.Doing, firstMessage.Hash);

            _application.EmitMessage(firstMessage);
            _application.EmitMessage(successor);

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.Doing);
        }

        [TestMethod]
        public void CardDeleted()
        {
            _application.EmitMessage(_application.Board.DeleteCard(_card));

            _application.Board.Cards.Count().Should().Be(0);
        }

        private Message CardTextChanged(string text, params MessageHash[] predecessors)
        {
            return Message.CreateMessage(
                "CardTextChanged",
                predecessors,
                _card.CardId,
                new { Value = text });
        }

        private Message CardMoved(Column column, params MessageHash[] predecessors)
        {
            return Message.CreateMessage(
                "CardMoved",
                predecessors,
                _card.CardId,
                new { Value = column });
        }
    }
}
