using Assisticant.Fields;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace FieldService.Models
{
    public class Technician : IMessageHandler
    {
        private readonly Guid _id;

        private Observable<ImmutableList<Visit>> _visits = new Observable<ImmutableList<Visit>>(
            ImmutableList<Visit>.Empty);

        public Technician(Guid id)
        {
            _id = id;
        }

        public IEnumerable<Visit> Visits
        {
            get { return _visits.Value; }
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
            if (message.Type == "Visit")
            {
                string homeId = message.Body.HomeId;
                var home = new Home(Guid.Parse(homeId));

                string incidentId = message.Body.IncidentId;
                var incident = new Incident(Guid.Parse(incidentId), home);

                string visitId = message.Body.VisitId;
                var visit = new Visit(Guid.Parse(visitId), incident);

                _visits.Value = _visits.Value.Add(visit);
            }
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }
    }
}
