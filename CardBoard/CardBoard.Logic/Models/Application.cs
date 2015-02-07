using Assisticant.Collections;
using Assisticant.Fields;
using CardBoard.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CardBoard.Models
{
    public class Application
    {
        private readonly IMessageStore _messageStore;
        private readonly IMessageQueue _messageQueue;
        private readonly IMessagePump _messagePump;

        private Board _board = new Board();
        private ComputedDictionary<Guid, IMessageHandler> _messageHandlers;

        private Observable<Exception> _exception = new Observable<Exception>();
        
        public Application(
            IMessageStore messageStore = null,
            IMessageQueue messageQueue = null,
            IMessagePump messagePump = null)
        {
            if (messageStore == null)
                messageStore = new NoOpMessageStore();
            if (messageQueue == null)
                messageQueue = new NoOpMessageQueue();
            if (messagePump == null)
                messagePump = new NoOpMessagePump();

            _messageStore = messageStore;
            _messageQueue = messageQueue;
            _messagePump = messagePump;

            _messageHandlers = new ComputedDictionary<Guid, IMessageHandler>(() =>
                new List <IMessageHandler> { _board }.Union(_board.Cards)
                    .ToDictionary(m => m.GetObjectId()));
        }

        public async void Load()
        {
            try
            {
                var boardMessages = await _messageStore.LoadAsync(_board.GetObjectId());
                _board.HandleAllMessages(boardMessages);
                foreach (var card in _board.Cards)
                {
                    var cardMessages = await _messageStore.LoadAsync(card.GetObjectId());
                    card.HandleAllMessages(cardMessages);
                }

                var queueMessages = await _messageQueue.LoadAsync();
                _messagePump.SendAllMessages(queueMessages);
            }
            catch (Exception ex)
            {
                _exception.Value = ex;
            }
        }

        public Board Board
        {
            get { return _board; }
        }

        public void EmitMessage(Message message)
        {
            _messageStore.Save(message);
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
