using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messaging
{
    public class MemoryMessageQueue : IMessageQueue
    {
        public void Confirm(Message message)
        {
        }

        public void Enqueue(Message message)
        {
        }

        public Exception Exception
        {
            get { return null; }
        }
    }
}
