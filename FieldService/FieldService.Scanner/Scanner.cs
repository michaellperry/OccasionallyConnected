using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.Scanner
{
    public class Scanner
    {
        private FileMessageQueue _queue;
        private HttpMessagePump _pump;

        private Task _task;
        private TaskCompletionSource<bool> _stopping;

        public Scanner(string queueFolderName, Uri distributorUri)
        {
            _queue = new FileMessageQueue(queueFolderName);
            _pump = new HttpMessagePump(distributorUri, _queue, new NoOpBookmarkStore());
        }

        public void Start()
        {
            if (_task != null)
                return;

            _stopping = new TaskCompletionSource<bool>();
            _task = Task.Run(() => ScannerProcess());
        }

        public void Stop()
        {
            if (_task == null)
                return;

            _stopping.SetResult(true);
            _task.Wait();
            _task = null;
            _stopping = null;
        }

        private async Task ScannerProcess()
        {
            try
            {
                var messages = await _queue.LoadAsync();
                _pump.SendAllMessages(messages);

                Console.WriteLine("Starting scanner:");
                while (await ShouldContinue(500))
                {
                    Console.WriteLine("Scanning...");
                }
                Console.WriteLine("Done.");
            }
            catch (Exception x)
            {
                Console.WriteLine(x.Message);
            }
        }

        private Task<bool> ShouldContinue(int timeout)
        {
            return Task.WhenAny(
                _stopping.Task       .ContinueWith(_ => false),
                Task.Delay(timeout)  .ContinueWith(_ => true))
                .ContinueWith(t => t.Result.Result);
        }
    }
}
