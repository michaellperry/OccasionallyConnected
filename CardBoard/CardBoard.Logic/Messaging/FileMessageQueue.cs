using CardBoard.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messaging
{
    public class FileMessageQueue : Process, IMessageQueue
    {
        public void Enqueue(Message message)
        {
            Perform(() => EnqueueAsync(message));
        }

        private Task EnqueueAsync(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
