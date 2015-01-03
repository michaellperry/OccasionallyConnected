using CardBoard.Messages;
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
            _application = new Application();
            var message = _application.Board.CreateCard();
            Guid cardId = Guid.Parse(message.Body.CardId);
            _application.ReceiveMessage(message);
            _card = _application.Board.Cards.Single(c => c.CardId == cardId);
        }

        [TestMethod]
        public void CardTextChanged()
        {
            _application.ReceiveMessage(CardTextChanged("Initial Text"));

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Initial Text");
        }

        [TestMethod]
        public void CardTextChangedAgain()
        {
            var firstMessage = CardTextChanged("Initial Text");
            var successor = CardTextChanged("New Text", firstMessage.Hash);

            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(successor);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void MessagesReceivedOutOfOrder()
        {
            var firstMessage = CardTextChanged("Initial Text");
            var successor = CardTextChanged("New Text", firstMessage.Hash);

            _application.ReceiveMessage(successor);
            _application.ReceiveMessage(firstMessage);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void CardMoved()
        {
            _application.ReceiveMessage(CardMoved(Column.ToDo));

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.ToDo);
        }

        [TestMethod]
        public void CardMovedAgain()
        {
            Message firstMessage = CardMoved(Column.ToDo);
            Message successor = CardMoved(Column.Doing, firstMessage.Hash);

            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(successor);

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.Doing);
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
