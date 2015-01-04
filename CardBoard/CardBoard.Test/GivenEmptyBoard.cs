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
            _application.HandleMessage(CardCreated(Guid.NewGuid()));

            _application.Board.Cards.Count().Should().Be(1);
        }

        [TestMethod]
        public void TwoCardsCreated()
        {
            _application.HandleMessage(CardCreated(Guid.NewGuid()));
            _application.HandleMessage(CardCreated(Guid.NewGuid()));

            _application.Board.Cards.Count().Should().Be(2);
        }

        [TestMethod]
        public void CardCreatedTwice()
        {
            Message message = CardCreated(Guid.NewGuid());
            _application.HandleMessage(message);
            _application.HandleMessage(message);

            _application.Board.Cards.Count().Should().Be(1);
        }

        [TestMethod]
        public void CardCreatedAndDeleted()
        {
            var cardId = Guid.NewGuid();
            _application.HandleMessage(CardCreated(cardId));
            _application.HandleMessage(CardDeleted(cardId));

            _application.Board.Cards.Count().Should().Be(0);
        }

        private static Message CardCreated(Guid cardId)
        {
            return Message.CreateMessage(
                "CardCreated",
                Guid.Empty,
                new
                {
                    CardId = cardId
                });
        }

        private static Message CardDeleted(Guid cardId)
        {
            return Message.CreateMessage(
                "CardDeleted",
                Guid.Empty,
                new
                {
                    CardId = cardId
                });
        }
    }
}
