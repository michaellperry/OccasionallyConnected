using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoverMob;
using RoverMob.Messaging;
using System.Threading.Tasks;

namespace FieldService.Bridge
{
    public class FieldServiceScanner : Scanner
    {
        private FileMessageQueue _queue;
        private HttpMessagePump _pump;

        public FieldServiceScanner(string queueFolderName, Uri distributorUri)
        {
            _queue = new FileMessageQueue(queueFolderName);
            _pump = new HttpMessagePump(distributorUri, _queue, new NoOpBookmarkStore());

            AddTableScanner("Home", r => HomeRecord.FromDataRow(r), OnInsertHome);
        }

        protected override async Task StartUp()
        {
            var messages = await _queue.LoadAsync();
            _pump.SendAllMessages(messages);
        }

        private void EmitMessage(Message message)
        {
            _queue.Enqueue(message);
            _pump.Enqueue(message);
        }

        private async Task OnInsertHome(HomeRecord record)
        {
            Guid homeId = Guid.NewGuid();

            EmitMessage(Message.CreateMessage(
                string.Empty,
                "Home",
                Guid.Empty,
                new
                {
                    HomeId = homeId
                }));

            EmitMessage(Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                homeId,
                new
                {
                    Value = record.Address
                }));
        }
    }
}
