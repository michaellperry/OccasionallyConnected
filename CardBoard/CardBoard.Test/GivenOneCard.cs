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
            _application = new Application();
            var cardId = Guid.NewGuid();
            _application.HandleMessage(_application.Board.CreateCard(cardId));
            _card = _application.Board.Cards.Single(c => c.CardId == cardId);
        }

        [TestMethod]
        public void CardTextChanged()
        {
            _application.HandleMessage(CardTextChanged("Initial Text"));

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Initial Text");
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
