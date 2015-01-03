using CardBoard.Messages;
using System.Linq;
using System;
using Assisticant.Fields;

namespace CardBoard.Models
{
    public class Application
    {
        private Board _board = new Board();
        private Observable<bool> _busy = new Observable<bool>();
        private Observable<string> _lastError = new Observable<string>();

        public Board Board
        {
            get { return _board; }
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public bool Busy
        {
            get { return _busy; }
            set { _busy.Value = value; }
        }

        public string LastError
        {
            get { return _lastError; }
            set { _lastError.Value = value; }
        }

        public void ReceiveMessage(Message message)
        {
            if (message.Type == "CardCreated")
                HandleCardCreated(message);

            else if (message.Type == "CardTextChanged")
                HandleCardTextChanged(message);

            else if (message.Type == "CardMoved")
                HandleCardMoved(message);
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

        private void HandleCardMoved(Message message)
        {
            foreach (var card in _board.Cards.Where(c => c.CardId == message.ObjectId))
                card.HandleCardMoved(message);
        }
    }
}
