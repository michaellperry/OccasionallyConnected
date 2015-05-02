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
            new MessageDispatcher<Technician>()
                .On("Visit", (t,m) => t.HandleVisit(m));

        private Observable<ImmutableList<Visit>> _visits =
            new Observable<ImmutableList<Visit>>(
                ImmutableList<Visit>.Empty);

        private readonly Guid _id;

        public Technician(Guid id)
        {
            _id = id;
        }

        public Message CreateVisit(DateTime startTime, DateTime endTime)
        {
            return Message.CreateMessage(
                _id.ToCanonicalString(),
                "Visit",
                _id,
                new
                {
                    VisitId = Guid.NewGuid(),
                    StartTime = startTime,
                    EndTime = endTime
                });
        }

        public IEnumerable<Visit> Visits
        {
            get
            {
                return _visits.Value;
            }
        }

        public Guid GetObjectId()
        {
            return _id;
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { yield break; }
        }

        public void HandleMessage(Message message)
        {
            _dispatcher.Dispatch(this, message);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
        }

        private void HandleVisit(Message message)
        {
            string visitId = message.Body.VisitId;
            DateTime startTime = message.Body.StartTime;
            DateTime endTime = message.Body.EndTime;

            _visits.Value = _visits.Value.Add(
                new Visit(visitId, startTime, endTime));
        }
    }
}
