using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messaging
{
    public interface IMessageQueue
    {
        void Enqueue(Message message);
        void Confirm(Message message);
        Exception Exception { get; }
    }
}
