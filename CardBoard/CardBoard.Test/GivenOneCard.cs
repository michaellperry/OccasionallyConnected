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
        public void CardAdded()
        {
            _application.EmitMessage(CardMoved(Column.ToDo));

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.ToDo);
        }

        [TestMethod]
        public void CardMoved()
        {
            Message predecessor = CardMoved(Column.ToDo);
            Message successor = CardMoved(Column.Doing, predecessor.Hash);
            _application.EmitMessage(predecessor);
            _application.EmitMessage(successor);

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.Doing);
        }

        [TestMethod]
        public void CardMovedAgain()
        {
            Message first = CardMoved(Column.ToDo);
            Message second = CardMoved(Column.Doing, first.Hash);
            Message third = CardMoved(Column.Done, second.Hash);
            _application.EmitMessage(first);
            _application.EmitMessage(second);
            _application.EmitMessage(third);

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.Done);
        }

        [TestMethod]
        public void ConflictDetected()
        {
            Message first = CardMoved(Column.ToDo);
            Message second = CardMoved(Column.Doing, first.Hash);
            Message fezzik = CardMoved(Column.Done, second.Hash);
            Message vizzini = CardMoved(Column.ToDo, second.Hash);
            _application.EmitMessage(first);
            _application.EmitMessage(second);
            _application.EmitMessage(fezzik);
            _application.EmitMessage(vizzini);

            _card.Column.Count().Should().Be(2);
            _card.Column.Should().Contain(c => c.Value == Column.ToDo);
            _card.Column.Should().Contain(c => c.Value == Column.Done);
        }

        [TestMethod]
        public void ConflictResolved()
        {
            Message first = CardMoved(Column.ToDo);
            Message second = CardMoved(Column.Doing, first.Hash);
            Message fezzik = CardMoved(Column.Done, second.Hash);
            Message vizzini = CardMoved(Column.ToDo, second.Hash);
            Message resolution = CardMoved(Column.ToDo,
                fezzik.Hash, vizzini.Hash);

            _application.EmitMessage(first);
            _application.EmitMessage(second);
            _application.EmitMessage(fezzik);
            _application.EmitMessage(vizzini);
            _application.EmitMessage(resolution);

            _card.Column.Count().Should().Be(1);
            _card.Column.Single().Value.Should().Be(Column.ToDo);
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
