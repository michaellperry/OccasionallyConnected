using Assisticant.Fields;
using CardBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.BoardView
{
    public class SelectionModel
    {
        private Observable<Card> _selectedCard = new Observable<Card>();

        public Card SelectedCard
        {
            get { return _selectedCard; }
            set { _selectedCard.Value = value; }
        }
    }
}
