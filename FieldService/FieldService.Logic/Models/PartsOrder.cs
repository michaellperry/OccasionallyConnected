using Assisticant.Fields;
using RoverMob;
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
        private readonly Guid _incidentId;
        private readonly string _description;

        private Observable<bool> _orderReceived =
            new Observable<bool>(false);
        
        public PartsOrder(Guid partsOrderId, Guid incidentId, string description)
        {
            _partsOrderId = partsOrderId;
            _incidentId = incidentId;
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
                _incidentId.ToCanonicalString(),
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
                _orderReceived.Value = true;
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public void HandleOrderReceived(Message message)
        {
            _orderReceived.Value = true;
        }
    }
}
