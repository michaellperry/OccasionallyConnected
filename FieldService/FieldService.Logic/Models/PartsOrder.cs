using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class PartsOrder : IMessageHandler
    {
        private static MessageDispatcher<PartsOrder> _dispatcher =
            new MessageDispatcher<PartsOrder>();

        private readonly Guid _partsOrderId;
        private readonly string _description;
        
        public PartsOrder(Guid partsOrderId, string description)
        {
            _partsOrderId = partsOrderId;
            _description = description;
        }

        public string Description
        {
            get { return _description; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { yield break; }
        }

        public Guid GetObjectId()
        {
            return _partsOrderId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }
    }
}
