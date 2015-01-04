using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messaging
{
    public class MemoryMessagePump : IMessagePump
    {
        public void Enqueue(CardBoard.Messaging.Message message)
        {
        }

        public void SendAllMessages(System.Collections.Immutable.ImmutableList<Message> messages)
        {
            throw new NotImplementedException();
        }
        public void SendAndReceiveMessages()
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
