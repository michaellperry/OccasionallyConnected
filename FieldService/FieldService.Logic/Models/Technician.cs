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
            get { return _visits.Items; }
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
            _visits.HandleMessage(message);
        }

        public void HandleAllMessages(IEnumerable<Message> messages)
        {
            _visits.HandleAllMessages(messages);
        }

        private Visit CreateVisitFromMessage(Message message)
        {
            string visitId = message.Body.VisitId;
            DateTime startTime = message.Body.StartTime;
            DateTime endTime = message.Body.EndTime;

            return new Visit(
                this,
                message.Hash,
                Guid.Parse(visitId),
                startTime,
                endTime);
        }
    }
}
