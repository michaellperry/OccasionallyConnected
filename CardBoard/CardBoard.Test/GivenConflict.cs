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
    public class GivenConflict
    {
        private Application _application;
        private Card _card;

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application();
            _application.ReceiveMessage(MessageFactory.CardCreated(Guid.NewGuid()));
            _card = _application.Board.Cards.Single();

            var firstMessage = MessageFactory.CardTextChanged(
                _card.CardId, "Initial Text", new List<MessageHash>());
            var parallelMessage = MessageFactory.CardTextChanged(
                _card.CardId, "New Text", new List<MessageHash>());
            _application.ReceiveMessage(firstMessage);
            _application.ReceiveMessage(parallelMessage);
        }

        [TestMethod]
        public void CardTextInConflict()
        {
            _card.Text.Count().Should().Be(2);
            _card.Text.Select(c => c.Value).Should().Contain("Initial Text");
            _card.Text.Select(c => c.Value).Should().Contain("New Text");
        }

        [TestMethod]
        public void CardTextResolveConflict()
        {
            var resolution = MessageFactory.CardTextChanged(
                _card.CardId, "Resolved Text",
                _card.Text.Select(c => c.MessageHash));
            _application.ReceiveMessage(resolution);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Resolved Text");
        }

        [TestMethod]
        public void CardTextConflictingResolutions()
        {
            var firstResolution = MessageFactory.CardTextChanged(
                _card.CardId, "First Resolution",
                _card.Text.Select(c => c.MessageHash));
            var secondResolution = MessageFactory.CardTextChanged(
                _card.CardId, "Second Resolution",
                _card.Text.Select(c => c.MessageHash));
            _application.ReceiveMessage(firstResolution);
            _application.ReceiveMessage(secondResolution);

            _card.Text.Count().Should().Be(2);
            _card.Text.Select(c => c.Value).Should().Contain("First Resolution");
            _card.Text.Select(c => c.Value).Should().Contain("Second Resolution");
        }
    }
}
