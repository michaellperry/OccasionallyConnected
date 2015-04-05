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

        private Mutable<string> _description = new Mutable<string>(string.Empty);
        
        public Incident(Guid incidentId, Home home)
        {
            _incidentId = incidentId;
            _home = home;
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
            get { throw new NotImplementedException(); }
        }

        public Guid GetObjectId()
        {
            throw new NotImplementedException();
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            throw new NotImplementedException();
        }

        public void HandleMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
