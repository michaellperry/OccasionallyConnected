using CardBoard.Messages;
using CardBoard.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
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
            _application.ReceiveMessage(MessageFactory.CardCreated(Guid.NewGuid()));
            _card = _application.Board.Cards.Single();
        }

        [TestMethod]
        public void CardTextChanged()
        {
            _application.ReceiveMessage(MessageFactory.CardTextChanged(
                _card.CardId, "Initial Text", new List<MessageHash>()));

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Initial Text");
        }

        [TestMethod]
        public void CardTextChangedAgain()
        {
            var firstMessage = MessageFactory.CardTextChanged(
                _card.CardId, "Initial Text", new List<MessageHash>());
            var successor = MessageFactory.CardTextChanged(
                _card.CardId, "New Text", new List<MessageHash> { firstMessage.Hash });
            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(successor);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void MessagesReceivedOutOfOrder()
        {
            var firstMessage = MessageFactory.CardTextChanged(
                _card.CardId, "Initial Text", new List<MessageHash>());
            var successor = MessageFactory.CardTextChanged(
                _card.CardId, "New Text", new List<MessageHash> { firstMessage.Hash });
            _application.ReceiveMessage(successor);
            _application.ReceiveMessage(firstMessage);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void CardMoved()
        {
            _application.ReceiveMessage(MessageFactory.CardMoved(
                _card.CardId, Column.ToDo, new List<MessageHash>()));

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.ToDo);
        }

        [TestMethod]
        public void CardMovedAgain()
        {
            _application.ReceiveMessage(MessageFactory.CardMoved(
                _card.CardId, Column.ToDo, new List<MessageHash>()));
            _application.ReceiveMessage(MessageFactory.CardMoved(
                _card.CardId, Column.Doing, _card.Column.Select(c => c.MessageHash)));

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.Doing);
        }
    }
}
