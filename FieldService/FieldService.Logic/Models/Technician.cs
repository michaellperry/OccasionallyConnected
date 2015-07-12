using Assisticant.Fields;
using RoverMob;
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
        private static MessageDispatcher<Technician> _dispatcher =
            new MessageDispatcher<Technician>();

        private SuccessorCollection<Visit> _visits;

        private readonly Guid _id;

        public Technician(Guid id)
        {
            _id = id;

            _visits = new SuccessorCollection<Visit>(
                "Visit", CreateVisitFromMessage,
                "Outcome", "visit");
        }

        public Message CreateVisit(
            Guid incidentId,
            Guid homeId,
            DateTime startTime,
            DateTime endTime)
        {
            return Message.CreateMessage(
                new TopicSet()
                    .Add(_id.ToCanonicalString())
                    .Add(incidentId.ToCanonicalString()),
                "Visit",
                Predecessors.Set,
                _id,
                new
                {
                    IncidentId = incidentId,
                    VisitId = Guid.NewGuid(),
                    HomeId = homeId,
                    StartTime = startTime,
                    EndTime = endTime
                });
        }

        public IEnumerable<Visit> Visits
        {
            get { return _visits.Items; }
        }

        public Guid GetObjectId()
        {
            return _id;
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { return _visits.Items; }
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
            _visits.HandleMessage(message);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _visits.HandleAllMessages(messages);
        }

        private Visit CreateVisitFromMessage(Message message)
        {
            string incidentId = message.Body.IncidentId;
            string visitId = message.Body.VisitId;
            string homeId = message.Body.HomeId;
            DateTime startTime = message.Body.StartTime;
            DateTime endTime = message.Body.EndTime;

            return new Visit(
                this,
                message.Hash,
                Guid.Parse(visitId),
                Guid.Parse(incidentId),
                Guid.Parse(homeId),
                startTime,
                endTime);
        }
    }
}
