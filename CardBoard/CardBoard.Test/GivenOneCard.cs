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
        public void CardTextChanged()
        {
            _application.EmitMessage(CardTextChanged("Initial Text"));

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Initial Text");
        }

        [TestMethod]
        public void CardTextChangedTwice()
        {
            Message predecessor = CardTextChanged("Initial Text");
            Message successor = CardTextChanged("Updated Text", predecessor.Hash);
            _application.EmitMessage(predecessor);
            _application.EmitMessage(successor);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Updated Text");
        }

        [TestMethod]
        public void CardTextConflict()
        {
            Message first = CardTextChanged("First Text");
            Message second = CardTextChanged("Second Text");
            _application.EmitMessage(first);
            _application.EmitMessage(second);

            _card.Text.Count().Should().Be(2);
            _card.Text.Select(c => c.Value).Should().Contain("First Text");
            _card.Text.Select(c => c.Value).Should().Contain("Second Text");
        }

        [TestMethod]
        public void CardTextConflictResolved()
        {
            Message first = CardTextChanged("First Text");
            Message second = CardTextChanged("Second Text");
            Message resolution = CardTextChanged("Resolved Text",
                first.Hash, second.Hash);
            _application.EmitMessage(first);
            _application.EmitMessage(second);
            _application.EmitMessage(resolution);

            _card.Text.Count().Should().Be(1);
            _card.Text.Select(c => c.Value).Should().Contain("Resolved Text");
        }

        [TestMethod]
        public void CardTextCanOccurOutOfOrder()
        {
            Message predecessor = CardTextChanged("Initial Text");
            Message successor = CardTextChanged("Updated Text",
                predecessor.Hash);
            _application.EmitMessage(successor);
            _application.EmitMessage(predecessor);

            _card.Text.Count().Should().Be(1);
            _card.Text.Single().Value.Should().Be("Updated Text");
        }

        [TestMethod]
        public void CardMoved()
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
