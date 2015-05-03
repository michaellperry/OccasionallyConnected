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

        public Incident(Guid incidentId, Guid homeId)
        {
            _incidentId = incidentId;
            _home = new Home(homeId);

            _description = new Mutable<string>(String.Empty);
        }

        public Home Home
        {
            get { return _home; }
        }

        public IEnumerable<Candidate<string>> Description
        {
            get { return _description.Candidates; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { yield return _home; }
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
