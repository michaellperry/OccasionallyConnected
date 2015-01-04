using CardBoard.Messaging;
using System.Linq;
using System;
using Assisticant.Fields;
using Assisticant.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace CardBoard.Models
{
    public class Application
    {
        private readonly IMessageQueue _messageQueue;
        private readonly IMessagePump _messagePump;
        private readonly IMessageStore _messageStore;

        private Board _board = new Board();

        private ComputedDictionary<Guid, IMessageHandler> _messageHandlers;
        private Observable<Exception> _exception = new Observable<Exception>();
        
        public Application(
            IMessageQueue messageQueue,
            IMessagePump messagePump,
            IMessageStore messageStore)
        {
            _messageQueue = messageQueue;
            _messagePump = messagePump;
            _messageStore = messageStore;

            _messageHandlers = new ComputedDictionary<Guid,IMessageHandler>(
                () => new List<IMessageHandler> { _board }.Union(_board.Cards)
                    .ToDictionary(h => h.GetObjectId()));
        }

        public async void Load()
        {
            try
            {
                ImmutableList<Message> boardMessages = await _messageStore.LoadAsync(_board.GetObjectId());
                _board.HandleAllMessages(boardMessages);
                foreach (var card in _board.Cards)
                {
                    ImmutableList<Message> cardMessages = await _messageStore.LoadAsync(card.GetObjectId());
                    card.HandleAllMessages(cardMessages);
                }

                ImmutableList<Message> queueMessages = await _messageQueue.LoadAsync();
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
                var exception =
                    _exception.Value ??
                    _messagePump.Exception ??
                    _messageQueue.Exception ??
                    _messageStore.Exception;
                return exception == null
                    ? null
                    : exception.Message;
            }
        }

        public void EmitMessage(Message message)
        {
            SendMessage(message);
            StoreMessage(message);
            HandleMessage(message);
        }

        private void SendMessage(Message message)
        {
            _messageQueue.Enqueue(message);
            _messagePump.Enqueue(message);
        }

        private void StoreMessage(Message message)
        {
            _messageStore.Save(message);
        }

        private void HandleMessage(Message message)
        {
            IMessageHandler messageHandler;
            if (_messageHandlers.TryGetValue(message.ObjectId, out messageHandler))
                messageHandler.HandleMessage(message);
        }
    }
}
