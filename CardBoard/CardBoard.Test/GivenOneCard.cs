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
            _application.EmitMessage(_application.Board.CreateCard(cardId));
            _card = _application.Board.Cards.Single(c => c.CardId == cardId);
        }

        [TestMethod]
        public void CardStartsInToDo()
        {
            _application.EmitMessage(CardMoved(Column.ToDo));

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.ToDo);
        }

        [TestMethod]
        public void CardMovedTwice()
        {
            Message predecessor = CardMoved(Column.ToDo);
            Message successor = CardMoved(Column.Doing, predecessor.Hash);
            _application.EmitMessage(predecessor);
            _application.EmitMessage(successor);

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.Doing);
        }

        private Message CardMoved(Column column, params MessageHash[] predecessors)
        {
            return Message.CreateMessage(
                "topic",
                "CardMoved",
                predecessors,
                _card.CardId,
                new
                {
                    Value = column
                });
        }
    }
}
