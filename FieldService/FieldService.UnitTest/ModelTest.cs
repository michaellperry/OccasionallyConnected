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
        [TestMethod]
        public void TechnicianHasOneVisit()
        {
            var technician = GivenTechnician();
            var application = new Application<Technician>();
            application.Load(technician);

            var homeId = Guid.NewGuid();
            var incidentId = Guid.NewGuid();
            var visitId = Guid.NewGuid();
            application.EmitMessage(Message.CreateMessage(
                technician.GetObjectId().ToCanonicalString(),
                "Visit",
                technician.GetObjectId(),
                new
                {
                    VisitId = visitId,
                    IncidentId = incidentId,
                    HomeId = homeId
                }));

            technician.Visits.Count().Should().Be(1);
            var visit = technician.Visits.Single();
            visit.GetObjectId().Should().Be(visitId);
            visit.Incident.Description.Count().Should().Be(0);
            visit.Incident.Home.Address.Count().Should().Be(0);
        }

        private static Technician GivenTechnician()
        {
            return new Technician(Guid.NewGuid());
        }
    }
}
