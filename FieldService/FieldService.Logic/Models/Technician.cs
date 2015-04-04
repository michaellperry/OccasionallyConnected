using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Technician : IMessageHandler
    {
        private readonly Guid _id;

        public Technician(Guid id)
        {
            _id = id;
        }

        public Guid GetObjectId()
        {
            return _id;
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { return Enumerable.Empty<IMessageHandler>(); }
        }

        public void HandleMessage(Message message)
        {
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }
    }
}
