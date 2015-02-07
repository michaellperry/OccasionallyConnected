using CardBoard.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace CardBoard.Messaging
{
    public class HttpMessagePump : Process, IMessagePump
    {
        private readonly Uri _uri;
        private readonly IMessageQueue _messageQueue;

        private ImmutableQueue<Message> _queue = ImmutableQueue<Message>.Empty;

        public HttpMessagePump(Uri uri, IMessageQueue messageQueue)
        {
            _uri = uri;
            _messageQueue = messageQueue;
        }

        public void Enqueue(Message message)
        {
            lock (this)
            {
                _queue = _queue.Enqueue(message);
            }
            Perform(() => SendAndReceiveMessagesInternalAsync());
        }

        private async Task SendAndReceiveMessagesInternalAsync()
        {
            using (HttpClient client = new HttpClient())
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
        }















        public void SendAndReceiveMessages()
        {
            throw new NotImplementedException();
        }

        public void SendAllMessages(ImmutableList<Message> messages)
        {
            throw new NotImplementedException();
        }

        private async Task SendMessageAsync(HttpClient client, Message message)
        {
            // TODO
            throw new CommunicationException("Simulated communication error.");
        }
    }
}
