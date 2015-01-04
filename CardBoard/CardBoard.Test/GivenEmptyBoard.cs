using CardBoard.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using CardBoard.Messaging;

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
        public void BoardHasNoCards()
        {
            _application.Board.Cards.Count().Should().Be(0);
        }

        [TestMethod]
        public void OneCardCreated()
        {
            _application.HandleMessage(_application.Board.CreateCard(Guid.NewGuid()));

            _application.Board.Cards.Count().Should().Be(1);
        }

        [TestMethod]
        public void TwoCardsCreated()
        {
            _application.HandleMessage(
                _application.Board.CreateCard(Guid.NewGuid()));
            _application.HandleMessage(
                _application.Board.CreateCard(Guid.NewGuid()));

            _application.Board.Cards.Count().Should().Be(2);
        }

        [TestMethod]
        public void CardCreatedTwice()
        {
            Message message = _application.Board.CreateCard(Guid.NewGuid());
            _application.HandleMessage(message);
            _application.HandleMessage(message);

            _application.Board.Cards.Count().Should().Be(1);
        }

        [TestMethod]
        public void CardCreatedAndDeleted()
        {
            var cardId = Guid.NewGuid();
            _application.HandleMessage(
                _application.Board.CreateCard(cardId));
            _application.HandleMessage(
                _application.Board.DeleteCard(cardId));

            _application.Board.Cards.Count().Should().Be(0);
        }
    }
}
