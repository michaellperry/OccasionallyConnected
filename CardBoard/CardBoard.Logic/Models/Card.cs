using CardBoard.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Models
{
    public class Card
    {
        private readonly Guid _cardId;

        public Card(Guid cardId)
        {
            _cardId = cardId;
        }

        public Guid CardId
        {
            get { return _cardId; }
        }
        public List<Candidate<string>> Text { get; set; }
        public List<Candidate<Column>> Column { get; set; }
    }
}
