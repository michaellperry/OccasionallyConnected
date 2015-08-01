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

        private MessageIdMap _messageIdMap;

        public FieldServiceScanner(string queueFolderName, Uri distributorUri)
        {
            _queue = new FileMessageQueue(queueFolderName);
            _pump = new HttpMessagePump(distributorUri, _queue, new NoOpBookmarkStore());

            _messageIdMap = new MessageIdMap();

            AddTableScanner("Home", r => HomeRecord.FromDataRow(r),
                OnInsertHome, OnUpdateHome);
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
            Console.WriteLine("Inserted home #{0} with address {1}.",
                record.HomeId, record.Address);

            Guid homeId = await _messageIdMap.GetOrCreateObjectId(
                "Home", record.HomeId);

            EmitMessage(Message.CreateMessage(
                string.Empty,
                "Home",
                Guid.Empty,
                new
                {
                    HomeId = homeId
                }));

            Message addressMessage = Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                homeId,
                new
                {
                    Value = record.Address
                });
            await _messageIdMap.SaveMessageHash(
                "Home", "Address", record.HomeId, record.Address, addressMessage.Hash);
            EmitMessage(addressMessage);
        }

        private async Task OnUpdateHome(HomeRecord oldValues, HomeRecord newValues)
        {
            Console.WriteLine("Updated home #{0} from address {1} to {2}.",
                oldValues.HomeId, oldValues.Address, newValues.Address);

            Guid homeId = await _messageIdMap.GetOrCreateObjectId(
                "Home", newValues.HomeId);
            MessageHash priorMessage = await _messageIdMap.GetMessageHash(
                "Home", "Address", oldValues.HomeId, oldValues.Address);

            Message message = Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                Predecessors.Set
                    .In("prior", priorMessage),
                homeId,
                new
                {
                    Value = newValues.Address
                });
            await _messageIdMap.SaveMessageHash(
                "Home", "Address", newValues.HomeId, newValues.Address, message.Hash);
            EmitMessage(message);
        }
    }
}
