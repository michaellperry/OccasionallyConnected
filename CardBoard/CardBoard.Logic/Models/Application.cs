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
        private Board _board = new Board();

        public Board Board
        {
            get { return _board; }
        }

        public void ReceiveMessage(Message message)
        {
            if (message.Type == "CardCreated")
            {
                HandleCardCreated(message);
            }
        }

        private void HandleCardCreated(Message message)
        {
            _board.NewCard();
        }
    }
}
