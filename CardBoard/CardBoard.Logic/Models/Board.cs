using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Models
{
    public class Board
    {
        private List<Card> _cards = new List<Card>();

        public IEnumerable<Card> Cards
        {
            get { return _cards; }
        }

        public void NewCard()
        {
            _cards.Add(new Card());
        }
    }
}
