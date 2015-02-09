using CardBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.BoardView
{
    public class BoardViewModel
    {
        private readonly Application _application;
        private readonly Board _board;
        private readonly SelectionModel _selection;
        
        public BoardViewModel(Application application, Board board, SelectionModel selection)
        {
            _application = application;
            _board = board;
            _selection = selection;
        }

        public IEnumerable<CardViewModel> ToDoCards
        {
            get { return CardsIn(Column.ToDo); }
        }

        public CardViewModel SelectedToDoCard
        {
            get { return SelectedCardIn(Column.ToDo); }
            set { SetSelectedCard(value); }
        }

        public IEnumerable<CardViewModel> DoingCards
        {
            get { return CardsIn(Column.Doing); }
        }

        public CardViewModel SelectedDoingCard
        {
            get { return SelectedCardIn(Column.Doing); }
            set { SetSelectedCard(value); }
        }

        public IEnumerable<CardViewModel> DoneCards
        {
            get { return CardsIn(Column.Done); }
        }

        public CardViewModel SelectedDoneCard
        {
            get { return SelectedCardIn(Column.Done); }
            set { SetSelectedCard(value); }
        }

        public static Uri UriOfCard(Card card)
        {
            return new Uri(
                String.Format("cardboard://card/{0}",
                    card.CardId),
                UriKind.Absolute);
        }

        public void MoveCard(Uri uri, Column column)
        {
            var cards = _board.Cards;
            var card = cards.FirstOrDefault(c => UriOfCard(c) == uri);
            if (card == null)
                return;

            _application.EmitMessage(card.MoveTo(column));
        }

        private IEnumerable<CardViewModel> CardsIn(Column column)
        {
            return
                from card in _board.Cards
                where card.Column.Any(c => c.Value == column)
                select new CardViewModel(card, column);
        }

        private CardViewModel SelectedCardIn(Column column)
        {
            if (_selection.SelectedCard == null)
                return null;

            if (!_selection.SelectedCard.Column.Any(c => c.Value == column))
                return null;

            return new CardViewModel(_selection.SelectedCard, column);
        }

        private void SetSelectedCard(CardViewModel cardViewModel)
        {
            _selection.SelectedCard = cardViewModel == null
                ? null
                : cardViewModel.Card;
        }
    }
}
