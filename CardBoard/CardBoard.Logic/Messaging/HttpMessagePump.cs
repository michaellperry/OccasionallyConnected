using Assisticant.Fields;
using CardBoard.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messaging
{
    public class HttpMessagePump : Process, IMessagePump
    {
        public void SendAndReceiveMessages()
        {
            Perform(() => SendAndReceiveMessagesAsync());
        }

        private async Task SendAndReceiveMessagesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
