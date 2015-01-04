using CardBoard.Messaging;
using System.Linq;
using System;
using Assisticant.Fields;
using Assisticant.Collections;
using System.Collections.Generic;

namespace CardBoard.Models
{
    public class Application
    {
        private readonly IMessageQueue _messageQueue;

        private Board _board = new Board();
        private Observable<bool> _busy = new Observable<bool>();
        private Observable<string> _lastError = new Observable<string>();

        private ComputedDictionary<Guid, IMessageHandler> _messageHandlers;

        public Application(IMessageQueue messageQueue)
        {
            _messageQueue = messageQueue;

            _messageHandlers = new ComputedDictionary<Guid,IMessageHandler>(
                () => new List<IMessageHandler> { _board }.Union(_board.Cards)
                    .ToDictionary(h => h.GetObjectId()));
        }

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

        public void EmitMessage(Message message)
        {
            _messageQueue.Enqueue(message);
            HandleMessage(message);
        }

        private void HandleMessage(Message message)
        {
            IMessageHandler messageHandler;
            if (_messageHandlers.TryGetValue(message.ObjectId, out messageHandler))
                messageHandler.HandleMessage(message);
        }
    }
}
