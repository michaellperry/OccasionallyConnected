using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messaging
{
    public class MemoryMessageQueue : IMessageQueue
    {
        private Queue<Message> _messages = new Queue<Message>();

        public void Enqueue(Message message)
        {
            _messages.Enqueue(message);
        }
    }
}
