using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardBoard.Models;
using System.Linq;
using FluentAssertions;
using CardBoard.Messages;
using System.Collections.Generic;

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
            var secondMessage = MessageFactory.CardTextChanged(
                _card.CardId, "New Text", new List<MessageHash> { firstMessage.Hash });
            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(secondMessage);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("New Text");
        }

        [TestMethod]
        public void CardTextInConflict()
        {
            var firstMessage = MessageFactory.CardTextChanged(
                _card.CardId, "Initial Text", new List<MessageHash>());
            var secondMessage = MessageFactory.CardTextChanged(
                _card.CardId, "New Text", new List<MessageHash>());
            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(secondMessage);

            _card.Text.Count().Should().Be(2);
            _card.Text.Select(c => c.Value).Should().Contain("Initial Text");
            _card.Text.Select(c => c.Value).Should().Contain("New Text");
        }

        [TestMethod]
        public void CardTextResolveConflict()
        {
            var firstMessage = MessageFactory.CardTextChanged(
                _card.CardId, "Initial Text", new List<MessageHash>());
            var secondMessage = MessageFactory.CardTextChanged(
                _card.CardId, "New Text", new List<MessageHash>());
            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(secondMessage);

            var resolutionMessage = MessageFactory.CardTextChanged(
                _card.CardId, "Resolved Text",
                _card.Text.Select(c => c.MessageHash));
            _application.ReceiveMessage(resolutionMessage);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Resolved Text");
        }
    }
}
