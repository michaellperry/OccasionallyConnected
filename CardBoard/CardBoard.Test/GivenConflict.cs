using CardBoard.Messaging;
using CardBoard.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;

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
            _application = new Application(new MemoryMessageQueue());
            var cardId = Guid.NewGuid();
            var message = _application.Board.CreateCard(cardId);
            _application.EmitMessage(message);
            _card = _application.Board.Cards.Single(c => c.CardId == cardId);

            var firstMessage = CardTextChanged("Initial Text");
            var parallelMessage = CardTextChanged("New Text");

            _application.EmitMessage(firstMessage);
            _application.EmitMessage(parallelMessage);
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
            var resolution = CardTextChanged("Resolved Text",
                _card.Text.Select(c => c.MessageHash));

            _application.EmitMessage(resolution);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Resolved Text");
        }

        [TestMethod]
        public void CardTextConflictingResolutions()
        {
            var firstResolution = CardTextChanged("First Resolution",
                _card.Text.Select(c => c.MessageHash));
            var secondResolution = CardTextChanged("Second Resolution",
                _card.Text.Select(c => c.MessageHash));

            _application.EmitMessage(firstResolution);
            _application.EmitMessage(secondResolution);

            _card.Text.Count().Should().Be(2);
            _card.Text.Select(c => c.Value).Should().Contain("First Resolution");
            _card.Text.Select(c => c.Value).Should().Contain("Second Resolution");
        }

        private Message CardTextChanged(string text, IEnumerable<MessageHash> predecessors = null)
        {
            return Message.CreateMessage(
                "CardTextChanged",
                predecessors ?? new List<MessageHash>(),
                _card.CardId,
                new { Value = text });
        }
    }
}
