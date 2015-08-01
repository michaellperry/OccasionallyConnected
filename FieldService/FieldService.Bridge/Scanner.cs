using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    public class Scanner : AsyncProcess
    {
        private FileMessageQueue _queue;
        private HttpMessagePump _pump;

        public Scanner(string queueFolderName, Uri distributorUri)
        {
            _queue = new FileMessageQueue(queueFolderName);
            _pump = new HttpMessagePump(distributorUri, _queue, new NoOpBookmarkStore());
        }

        protected override async Task StartUp()
        {
            var messages = await _queue.LoadAsync();
            _pump.SendAllMessages(messages);
        }

        protected override void DoWork()
        {
            throw new NotImplementedException();
        }
    }
}
