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

        public void SendAllMessages(ImmutableList<Message> messages)
        {
            throw new NotImplementedException();
        }

















        public void Enqueue(Message message)
        {
            throw new NotImplementedException();
        }

        public void SendAndReceiveMessages()
        {
            throw new NotImplementedException();
        }

        private async Task SendMessageAsync(HttpClient client, Message message)
        {
            // TODO
        }
    }
}
