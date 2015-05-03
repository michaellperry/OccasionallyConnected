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
    public class GivenTechnician
    {
        private Application<Technician> _application;
        private Technician _technician;

        [TestMethod]
        public void CanAddVisit()
        {
            Message createVisit = _technician.CreateVisit(
                Guid.Empty,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0));
            _application.EmitMessage(createVisit);

            _technician.Visits.Count().Should().Be(1);
        }

        [TestMethod]
        public void CanRemoveVisit()
        {
            Message createVisit = _technician.CreateVisit(
                Guid.Empty,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0));
            _application.EmitMessage(createVisit);

            var visit = _technician.Visits.Single();

            _application.EmitMessage(visit.CreateOutcome());

            _technician.Visits.Count().Should().Be(0);
        }

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application<Technician>();
            _technician = new Technician(Guid.NewGuid());
            _application.Load(_technician);
        }
    }
}
