using System;
using System.Collections.Immutable;

namespace CardBoard.Messaging
{
    public class NoOpMessagePump : IMessagePump
    {
        public void Enqueue(Message message)
        {
        }

        public void SendAndReceiveMessages()
        {
        }

        public void SendAllMessages(ImmutableList<Message> messages)
        {
        }

        public bool Busy
        {
            get { return false; }
        }

        public Exception Exception
        {
            get { return null; }
        }
    }
}
