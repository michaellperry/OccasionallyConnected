using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Incident : IMessageHandler
    {
        private readonly Guid _incidentId;
        private readonly Home _home;

        private Mutable<string> _description;
        private SuccessorCollection<PartsOrder> _partsOrders;

        public Incident(Guid incidentId, Guid homeId)
        {
            _incidentId = incidentId;
            _home = new Home(homeId);

            _description = new Mutable<string>(String.Empty);
            _partsOrders = new SuccessorCollection<PartsOrder>(
                "PartsOrder", CreatePartsOrderFromMessage,
                "PartsOrderCancelled", "partsOrder");
        }

        public Home Home
        {
            get { return _home; }
        }

        public IEnumerable<Candidate<string>> Description
        {
            get { return _description.Candidates; }
        }

        public IEnumerable<PartsOrder> PartsOrders
        {
            get { return _partsOrders.Items; }
        }

        public Message CreatePartsOrder(string description)
        {
            return Message.CreateMessage(
                string.Empty,
                "PartsOrder",
                _incidentId,
                new
                {
                    PartsOrderId = Guid.NewGuid(),
                    Description = description
                });
        }

        public IEnumerable<IMessageHandler> Children
        {
            get
            {
                return new List<IMessageHandler>() { _home }
                    .Concat(_partsOrders.Items);
            }
        }

        public Guid GetObjectId()
        {
            return _incidentId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _description.HandleAllMessages(messages
                .Where(m => m.Type == "IncidentDescription"));
            _partsOrders.HandleAllMessages(messages);
        }

        public void HandleMessage(Message message)
        {
            if (message.Type == "IncidentDescription")
                _description.HandleMessage(message);
            _partsOrders.HandleMessage(message);
        }

        private PartsOrder CreatePartsOrderFromMessage(Message message)
        {
            string partsOrderId = message.Body.PartsOrderId;
            string description = message.Body.Description;

            return new PartsOrder(Guid.Parse(partsOrderId), description);
        }
    }
}
