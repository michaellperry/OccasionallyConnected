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
            _application.HandleMessage(Message.CreateMessage(
                "CardCreated",
                Guid.Empty,
                new
                {
                    CardId = Guid.NewGuid()
                }));

            _application.Board.Cards.Count().Should().Be(1);
        }
    }
}
