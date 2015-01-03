using Assisticant.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.BoardView
{
    public class CardDetailModel
    {
        private Observable<string> _text = new Observable<string>();

        public string Text
        {
            get { return _text.Value; }
            set { _text.Value = value; }
        }
    }
}
