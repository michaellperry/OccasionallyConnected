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
        private readonly IMessagePump _messagePump;

        private Board _board = new Board();

        private ComputedDictionary<Guid, IMessageHandler> _messageHandlers;
        
        public Application(IMessageQueue messageQueue, IMessagePump messagePump)
        {
            _messageQueue = messageQueue;
            _messagePump = messagePump;

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
            _messagePump.SendAndReceiveMessages();
        }

        public bool Busy
        {
            get { return _messagePump.Busy; }
        }

        public string LastError
        {
            get
            {
                return _messagePump.Exception == null
                    ? null
                    : _messagePump.Exception.Message;
            }
        }

        public void EmitMessage(Message message)
        {
            _messageQueue.Enqueue(message);
            _messagePump.SendAndReceiveMessages();
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
