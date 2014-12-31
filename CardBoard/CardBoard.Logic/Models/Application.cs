using CardBoard.Messages;
using System.Linq;

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
                HandleCardCreated(message);

            else if (message.Type == "CardTextChanged")
                HandleCardTextChanged(message);
        }

        private void HandleCardCreated(Message message)
        {
            _board.HandleCardCreated(message);
        }

        private void HandleCardTextChanged(Message message)
        {
            foreach (var card in _board.Cards.Where(c => c.CardId == message.ObjectId))
                card.HandleCardTextChanged(message);
        }
    }
}
