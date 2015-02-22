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
        private readonly IPushNotificationSubscription _pushNotificationSubscription;

        private Board _board = new Board();
        private ComputedDictionary<Guid, IMessageHandler> _messageHandlers;

        private Observable<Exception> _exception = new Observable<Exception>();
        
        public Application() : this(
            new NoOpMessageStore(),
            new NoOpMessageQueue(),
            new NoOpMessagePump(),
            new NoOpPushNotificationSubscription())
        {
        }

        public Application(
            IMessageStore messageStore,
            IMessageQueue messageQueue,
            IMessagePump messagePump,
            IPushNotificationSubscription pushNotificationSubscription)
        {
            _messageStore = messageStore;
            _messageQueue = messageQueue;
            _messagePump = messagePump;
            _pushNotificationSubscription = pushNotificationSubscription;

            _messagePump.MessageReceived += MessageReceived;
            _pushNotificationSubscription.MessageReceived += NotificationReceived;

            _messageHandlers = new ComputedDictionary<Guid, IMessageHandler>(() =>
                new List <IMessageHandler> { _board }.Union(_board.Cards)
                    .ToDictionary(m => m.GetObjectId()));
        }

        public async void Load()
        {
            try
            {
                _messagePump.Subscribe(_board.GetObjectId().ToCanonicalString());
                await _pushNotificationSubscription.Subscribe(
                    _board.GetObjectId().ToCanonicalString());

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

        public Exception Exception
        {
            get
            {
                return
                    _exception.Value ??
                    _messageQueue.Exception ??
                    _messagePump.Exception;
            }
        }

        public void SendAndReceiveMessages()
        {
            _messagePump.SendAndReceiveMessages();
        }

        public void EmitMessage(Message message)
        {
            _messageStore.Save(message);
            _messageQueue.Enqueue(message);
            _messagePump.Enqueue(message);
            HandleMessage(message);
        }

        private void MessageReceived(Message message)
        {
            _messageStore.Save(message);
            HandleMessage(message);
        }

        private void NotificationReceived(Message message)
        {
            _messageStore.Save(message);
            HandleMessage(message);
            _messagePump.SendAndReceiveMessages();
        }

        private void HandleMessage(Message message)
        {
            IMessageHandler messageHandler;
            if (_messageHandlers.TryGetValue(message.ObjectId, out messageHandler))
                messageHandler.HandleMessage(message);
        }
    }
}
