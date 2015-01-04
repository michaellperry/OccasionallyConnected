using CardBoard.Tasks;
using Newtonsoft.Json;
using System;
using System.Collections.Immutable;
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
            Perform(() => SendAndReceiveMessagesAsync());
        }

        public void SendAndReceiveMessages()
        {
            Perform(() => SendAndReceiveMessagesAsync());
        }

        private async Task SendAndReceiveMessagesAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                while (true)
                {
                    Message message;
                    lock (this)
                    {
                        if (_queue.IsEmpty)
                            return;
                        _queue = _queue.Dequeue(out message);
                    }

                    dynamic memento = message.GetMemento();
                    string messageJson = JsonConvert.SerializeObject(memento);

                    IHttpContent content = new HttpStringContent(messageJson);
                    var result = await client.PostAsync(_uri, content);
                    if (!result.IsSuccessStatusCode)
                        return;

                    _messageQueue.Confirm(message);
                }
            }
        }
    }
}
