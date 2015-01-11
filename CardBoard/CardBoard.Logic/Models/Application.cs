using Assisticant.Collections;
using CardBoard.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Application
    {
        private readonly IMessageStore _messageStore;

        private Board _board = new Board();
        private ComputedDictionary<Guid, IMessageHandler> _messageHandlers;
        
        public Application(IMessageStore messageStore = null)
        {
            if (messageStore == null)
                messageStore = new NoOpMessageStore();

            _messageStore = messageStore;

            _messageHandlers = new ComputedDictionary<Guid, IMessageHandler>(() =>
                new List <IMessageHandler> { _board }.Union(_board.Cards)
                    .ToDictionary(m => m.GetObjectId()));
        }

        public Board Board
        {
            get { return _board; }
        }

        public void EmitMessage(Message message)
        {
            _messageStore.Save(message);
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
