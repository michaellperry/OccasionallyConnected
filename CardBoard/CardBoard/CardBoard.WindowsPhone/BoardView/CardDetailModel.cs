using Assisticant.Fields;
using CardBoard.Models;
using System;
using System.Linq;

namespace CardBoard.BoardView
{
    public class CardDetailModel
    {
        private Observable<string> _text = new Observable<string>();
        private Observable<Column> _selectedColumn = new Observable<Column>();

        public string Text
        {
            get { return _text.Value; }
            set { _text.Value = value; }
        }

        public Column SelectedColumn
        {
            get { return _selectedColumn.Value; }
            set { _selectedColumn.Value = value; }
        }

        public void Clear()
        {
            _text.Value = String.Empty;
            _selectedColumn.Value = Column.ToDo;
        }

        public void FromCard(Card card)
        {
            _text.Value = card.Text
                .OrderBy(c => c.MessageHash)
                .Select(c => c.Value)
                .FirstOrDefault();
            _selectedColumn.Value = card.Column
                .OrderBy(c => c.MessageHash)
                .Select(c => c.Value)
                .FirstOrDefault();
        }
    }
}
