using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messaging
{
    public interface IMessagePump
    {
        void SendAndReceiveMessages();
        bool Busy { get; }
        Exception Exception { get; }
    }
}
