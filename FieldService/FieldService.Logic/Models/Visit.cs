using Assisticant.Fields;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Visit : IMessageHandler
    {
        private readonly Guid _visitId;
        private readonly Incident _incident;
        
        public Visit(Guid visitId, Incident incident)
        {
            _visitId = visitId;
            _incident = incident;
        }

        public Incident Incident
        {
            get { return _incident; }
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { return new List<IMessageHandler>() { _incident }; }
        }

        public Guid GetObjectId()
        {
            return _visitId;
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        public void HandleMessage(Message message)
        {
        }
    }
}
