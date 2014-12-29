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
            else if (message.Type == "CardTextChanged")
            {
                HandleCardTextChanged(message);
            }
        }

        private void HandleCardCreated(Message message)
        {
            var cardId = message.Body.Value<Guid>("CardId");
            if (!_board.Cards.Any(c => c.CardId == cardId))
                _board.NewCard(cardId);
        }

        private void HandleCardTextChanged(Message message)
        {
            var cardId = message.ObjectId;
            var value = message.Body.Value<string>("Value");
            foreach (var card in _board.Cards.Where(c => c.CardId == cardId))
            {
                card.SetCardText(message.Hash, value, message.Predecessors);
            }
        }
    }
}
