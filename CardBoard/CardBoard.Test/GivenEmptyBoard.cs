using CardBoard.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Linq;

namespace CardBoard.Test
{
    [TestClass]
    public class GivenEmptyBoard
    {
        private Application _application;

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application();
        }

        [TestMethod]
        public void OneCardCreated()
        {
            _application.ReceiveMessage(MessageFactory.CardCreated(Guid.NewGuid()));

            _application.Board.Cards.Count().Should().Be(1);
        }

        [TestMethod]
        public void TwoCardsCreated()
        {
            _application.ReceiveMessage(MessageFactory.CardCreated(Guid.NewGuid()));
            _application.ReceiveMessage(MessageFactory.CardCreated(Guid.NewGuid()));

            _application.Board.Cards.Count().Should().Be(2);
        }

        [TestMethod]
        public void CardCreatedTwice()
        {
            Guid cardId = Guid.NewGuid();
            _application.ReceiveMessage(MessageFactory.CardCreated(cardId));
            _application.ReceiveMessage(MessageFactory.CardCreated(cardId));

            _application.Board.Cards.Count().Should().Be(1);
        }
    }
}
