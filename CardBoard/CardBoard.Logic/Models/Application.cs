using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardBoard.Messages;

namespace CardBoard.Models
{
    public class Application
    {
        public Board Board { get; set; }

        public void ReceiveMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
