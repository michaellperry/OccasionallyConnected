using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardBoard.Models;

namespace CardBoard.BoardView
{
    public class BoardViewModel
    {
        private readonly Application _application;
        private readonly Board _board;
        
        public BoardViewModel(Application application, Board board)
        {
            _application = application;
            _board = board;
        }

        public IEnumerable<CardViewModel> ToDoCards
        {
            get { return CardsIn(Column.ToDo); }
        }

        public IEnumerable<CardViewModel> DoingCards
        {
            get { return CardsIn(Column.Doing); }
        }

        public IEnumerable<CardViewModel> DoneCards
        {
            get { return CardsIn(Column.Done); }
        }

        private IEnumerable<CardViewModel> CardsIn(Column column)
        {
            return
                from card in _board.Cards
                where card.Column.Any(c => c.Value == column)
                select new CardViewModel(card, column);
        }
    }
}
