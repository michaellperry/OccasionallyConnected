using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoverMob;

namespace FieldService.Models
{
    public class Visit : IMessageHandler
    {
        private readonly Technician _technician;
        private readonly MessageHash _visitHash;
        private readonly Guid _visitId;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;

        private readonly Incident _incident;
        
        public Visit(
            Technician technician,
            MessageHash visitHash,
            Guid visitId,
            Guid incidentId,
            Guid homeId,
            DateTime startTime,
            DateTime endTime)
        {
            _technician = technician;
            _visitHash = visitHash;
            _visitId = visitId;
            _startTime = startTime;
            _endTime = endTime;

            _incident = new Incident(incidentId, homeId);
        }

        public MessageHash VisitHash
        {
            get { return _visitHash; }
        }

        public DateTime StartTime
        {
            get { return _startTime; }
        }

        public DateTime EndTime
        {
            get { return _endTime; }
        }

        public Incident Incident
        {
            get { return _incident; }
        }

        public Message CreateOutcome()
        {
            return Message.CreateMessage(
                new TopicSet()
                    .Add(_technician.GetObjectId().ToCanonicalString())
                    .Add(_incident.GetObjectId().ToCanonicalString()),
                "Outcome",
                Predecessors.Set.In("visit", _visitHash),
                _technician.GetObjectId(),
                new
                {

                });
        }

        public IEnumerable<IMessageHandler> Children
        {
            get { yield return _incident; }
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
