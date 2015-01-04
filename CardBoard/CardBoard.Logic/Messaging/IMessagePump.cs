using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Immutable;

namespace CardBoard.Messaging
{
    public interface IMessagePump
    {
        void Enqueue(Message message);
        void SendAndReceiveMessages();
        void SendAllMessages(ImmutableList<Message> messages);
        bool Busy { get; }
        Exception Exception { get; }
    }
}
