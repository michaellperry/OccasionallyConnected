using CardBoard.Models;
using System;
using System.Linq;

namespace CardBoard.BoardView
{
    public class CardViewModel
    {
        private readonly Card _card;
        private readonly Column _column;
        
        public CardViewModel(Card card, Column column)
        {
            _card = card;
            _column = column;
        }

        public Card Card
        {
            get { return _card; }
        }

        public string Text
        {
            get
            {
                return _card.Text
                    .OrderBy(c => c.MessageHash)
                    .Select(c => c.Value)
                    .FirstOrDefault();
            }
        }

        public bool Conflict
        {
            get
            {
                return _card.Column
                    .Select(c => c.Value)
                    .Distinct()
                    .Count() > 1;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var that = (CardViewModel)obj;
            return
                Object.Equals(this._card, that._card) &&
                this._column == that._column;
        }

        public override int GetHashCode()
        {
            return _card.GetHashCode() * 3 + (int)_column;
        }

    }
}
