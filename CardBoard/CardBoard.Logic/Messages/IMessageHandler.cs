using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardBoard.Messages
{
    public interface IMessageHandler
    {
        Guid GetObjectId();
        void HandleMessage(Message message);
    }
}
