using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CardBoard.Models;
using System.Linq;
using FluentAssertions;

namespace CardBoard.Test
{
    [TestClass]
    public class GivenOneCard
    {
        private Application _application;

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application();
            _application.ReceiveMessage(MessageFactory.CardCreated(Guid.NewGuid()));
        }

        [TestMethod]
        public void CardTextChanged()
        {
            var card = _application.Board.Cards.Single();
            _application.ReceiveMessage(MessageFactory.CardTextChanged(
                card.CardId, "Initial Text"));

            card.Text.Count().Should().Be(1);
            card.Text.Single().Value.Should().Be("Initial Text");
        }
    }
}
