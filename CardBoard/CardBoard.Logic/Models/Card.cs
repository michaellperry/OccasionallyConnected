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
        public Guid Id { get; set; }
        public List<Candidate<string>> Text { get; set; }
        public List<Candidate<Column>> Column { get; set; }
    }
}
