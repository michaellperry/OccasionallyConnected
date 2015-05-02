using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using FieldService.Models;
using RoverMob.Messaging;
using RoverMob;
using FluentAssertions;

namespace FieldService.UnitTest
{
    [TestClass]
    public class ModelTest
    {
        private Application<Technician> _application;
        private Technician _technician;

        [TestMethod]
        public void TechnicianHasOneVisit()
        {
            Guid technicianId = GivenTechnician();
            Guid homeId = GivenHome();
            Guid incidentId = GivenIncident(homeId);
            Guid visitId = GivenVisit(
                technicianId,
                homeId,
                incidentId,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0));

            _technician.Visits.Count().Should().Be(1);
            var visit = _technician.Visits.Single();
            visit.GetObjectId().Should().Be(visitId);
            visit.Incident.Description.Count().Should().Be(0);
            visit.Incident.Home.Address.Count().Should().Be(0);
        }

        [TestMethod]
        public void VisitLoadsIncident()
        {
            Guid technicianId = GivenTechnician();
            Guid homeId = GivenHome();
            Guid incidentId = GivenIncident(homeId);
            GivenIncidentDescription(incidentId, "Garbage disposal clogged");
            Guid visitId = GivenVisit(
                technicianId,
                homeId,
                incidentId,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0));

            var visit = _technician.Visits.Single(v => v.GetObjectId() == visitId);
            visit.Incident.Description.Count().Should().Be(1);
            visit.Incident.Description.Single().Value.Should().Be("Garbage disposal clogged");
        }

        [TestMethod]
        public void VisitLoadsHome()
        {
            Guid technicianId = GivenTechnician();
            Guid homeId = GivenHome();
            GivenHomeAddress(homeId, "121B Baker Street");
            Guid incidentId = GivenIncident(homeId);
            Guid visitId = GivenVisit(
                technicianId,
                homeId,
                incidentId,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0));

            var visit = _technician.Visits.Single(v => v.GetObjectId() == visitId);
            visit.Incident.Home.Address.Count().Should().Be(1);
            visit.Incident.Home.Address.Single().Value.Should().Be("121B Baker Street");
        }

        private Guid GivenTechnician()
        {
            _application = new Application<Technician>();
            var technicianId = Guid.NewGuid();
            _technician = new Technician(technicianId);
            _application.Load(_technician);
            return technicianId;
        }

        private static Guid GivenHome()
        {
            var homeId = Guid.NewGuid();
            return homeId;
        }

        private void GivenHomeAddress(Guid homeId, string value)
        {
            _application.EmitMessage(Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                homeId,
                new
                {
                    Value = value
                }));
        }

        private Guid GivenIncident(Guid homeId)
        {
            var incidentId = Guid.NewGuid();
            return incidentId;
        }

        private void GivenIncidentDescription(Guid incidentId, string value)
        {
            _application.EmitMessage(Message.CreateMessage(
                incidentId.ToCanonicalString(),
                "IncidentDescription",
                incidentId,
                new
                {
                    Value = value
                }));

            if (_application.Exception != null)
                throw _application.Exception;
        }

        private Guid GivenVisit(
            Guid technicianId,
            Guid homeId,
            Guid incidentId,
            DateTime startTime,
            DateTime endTime)
        {
            var visitId = Guid.NewGuid();
            _application.EmitMessage(Message.CreateMessage(
                technicianId.ToCanonicalString(),
                "Visit",
                technicianId,
                new
                {
                    VisitId = visitId,
                    IncidentId = incidentId,
                    HomeId = homeId,
                    StartTime = startTime,
                    EndTime = endTime
                }));

            if (_application.Exception != null)
                throw _application.Exception;

            return visitId;
        }
    }
}
