using FieldService.Models;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace FieldService.UnitTest
{
    [TestClass]
    public class GivenVisit
    {
        private Application<Technician> _application;
        private Visit _visit;

        [TestMethod]
        public void VisitHasAnIncident()
        {
            Incident incident = _visit.Incident;

            string description = incident.Description.Single().Value;
            description.Should().Be("Garbage disposal jammed");
        }

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application<Technician>();
            var technician = new Technician(Guid.NewGuid());
            _application.Load(technician);
            Guid incidentId = Guid.NewGuid();
            _application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "Incident",
                Guid.Empty,
                new
                {
                    IncidentId = incidentId
                }));
            _application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "IncidentDescription",
                incidentId,
                new
                {
                    Value = "Garbage disposal jammed"
                }));
            _application.EmitMessage(technician.CreateVisit(
                incidentId,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0)));

            _visit = technician.Visits.Single();
        }
    }
}
