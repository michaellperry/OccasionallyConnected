using CardBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.BoardView
{
    public class CardDetailViewModel
    {
        private readonly CardDetailModel _cardDetail;
        private readonly Application _application;
        private readonly Card _card;

        public CardDetailViewModel(CardDetailModel cardDetail, Application application, Card card)
        {
            _cardDetail = cardDetail;
            _application = application;
            _card = card;
        }

        public string Text
        {
            get { return _cardDetail.Text; }
            set { _cardDetail.Text = value; }
        }

        public IEnumerable<ColumnViewModel> Columns
        {
            get
            {
                yield return new ColumnViewModel(Column.ToDo);
                yield return new ColumnViewModel(Column.Doing);
                yield return new ColumnViewModel(Column.Done);
            }
        }

        public ColumnViewModel SelectedColumn
        {
            get
            {
                return _cardDetail.SelectedColumn == null ? null
                    : new ColumnViewModel(_cardDetail.SelectedColumn);
            }
            set
            {
                if (value != null)
                    _cardDetail.SelectedColumn = value.Column;
            }
        }

        public void Ok()
        {
            var card = _card;
            if (card == null)
            {
                var cardId = Guid.NewGuid();
                _application.EmitMessage(_application.Board.CreateCard(cardId));
                card = _application.Board.Cards.Single(c => c.CardId == cardId);
            }

            if (card.Text.Count() != 1 ||
                card.Text.Single().Value != _cardDetail.Text)
                _application.EmitMessage(card.SetText(_cardDetail.Text));
            if (card.Column.Count() != 1 ||
                card.Column.Single().Value != _cardDetail.SelectedColumn)
                _application.EmitMessage(card.MoveTo(_cardDetail.SelectedColumn));
        }
    }
}
