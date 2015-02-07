using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void SendAllMessages(System.Collections.Immutable.ImmutableList<Message> messages)
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
