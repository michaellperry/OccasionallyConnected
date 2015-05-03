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

        private Mutable<string> _description;

        public Incident(Guid incidentId)
        {
            _incidentId = incidentId;

            _description = new Mutable<string>(String.Empty);
        }

        public IEnumerable<Candidate<string>> Description
        {
            get { return _description.Candidates; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { yield break; }
        }

        public Guid GetObjectId()
        {
            return _incidentId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _description.HandleAllMessages(messages
                .Where(m => m.Type == "IncidentDescription"));
        }

        public void HandleMessage(Message message)
        {
            if (message.Type == "IncidentDescription")
                _description.HandleMessage(message);
        }
    }
}
