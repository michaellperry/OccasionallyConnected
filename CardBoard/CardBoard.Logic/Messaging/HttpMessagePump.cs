using CardBoard.Protocol;
using CardBoard.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace CardBoard.Messaging
{
    public class HttpMessagePump : Process, IMessagePump
    {
        private readonly Uri _uri;
        private readonly IMessageQueue _messageQueue;
        private readonly IBookmarkStore _bookmarkStore;

        private ImmutableQueue<Message> _queue = ImmutableQueue<Message>.Empty;
        
        public HttpMessagePump(
            Uri uri,
            IMessageQueue messageQueue,
            IBookmarkStore bookmarkStore)
        {
            _uri = uri;
            _messageQueue = messageQueue;
            _bookmarkStore = bookmarkStore;
        }

        public void SendAllMessages(ImmutableList<Message> messages)
        {
            lock (this)
            {
                foreach (var message in messages)
                    _queue = _queue.Enqueue(message);
            }
            Perform(() => SendAndReceiveMessagesInternalAsync());
        }

        public void Enqueue(Message message)
        {
            lock (this)
            {
                _queue = _queue.Enqueue(message);
            }
            Perform(() => SendAndReceiveMessagesInternalAsync());
        }

        public void SendAndReceiveMessages()
        {
            Perform(() => SendAndReceiveMessagesInternalAsync());
        }

        private async Task SendAndReceiveMessagesInternalAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                await SendMessagesInternalAsync(client);
            }
        }

        private async Task SendMessagesInternalAsync(HttpClient client)
        {
            while (true)
            {
                var queue = _queue;
                if (queue.IsEmpty)
                    return;
                var message = queue.Peek();

                await SendMessageAsync(client, message);

                lock (this)
                {
                    _queue = _queue.Dequeue();
                }
                _messageQueue.Confirm(message);
            }
        }

        private async Task SendMessageAsync(HttpClient client, Message message)
        {
            throw new NotImplementedException();
        }
    }
}
