﻿using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoverMob;

namespace FieldService.Models
{
    public class Visit
    {
        private readonly Technician _technician;
        private readonly MessageHash _visitHash;
        private readonly Guid _visitId;
        private readonly DateTime _startTime;
        private readonly DateTime _endTime;
        
        public Visit(
            Technician technician,
            MessageHash visitHash,
            Guid visitId,
            DateTime startTime,
            DateTime endTime)
        {
            _technician = technician;
            _visitHash = visitHash;
            _visitId = visitId;
            _startTime = startTime;
            _endTime = endTime;
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

        public Message CreateOutcome()
        {
            return Message.CreateMessage(
                _technician.GetObjectId().ToCanonicalString(),
                "Outcome",
                Predecessors.Set.In("visit", _visitHash),
                _technician.GetObjectId(),
                new
                {

                });
        }
    }
}