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
            new MessageDispatcher<PartsOrder>()
                .On("OrderReceived", (po,m) => po.HandleOrderReceived(m));

        private readonly Guid _partsOrderId;
        private readonly string _description;

        private bool _orderReceived = false;
        
        public PartsOrder(Guid partsOrderId, string description)
        {
            _partsOrderId = partsOrderId;
            _description = description;
        }

        public string Description
        {
            get { return _description; }
        }

        public bool OrderReceived
        {
            get
            {
                return _orderReceived;
            }
        }

        public Message Receive()
        {
            return Message.CreateMessage(
                string.Empty,
                "OrderReceived",
                _partsOrderId,
                new { });
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
            if (messages.Any(m => m.Type == "OrderReceived"))
                _orderReceived = true;
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public void HandleOrderReceived(Message message)
        {
            _orderReceived = true;
        }
    }
}
