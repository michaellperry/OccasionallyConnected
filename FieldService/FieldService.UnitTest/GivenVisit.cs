﻿using FieldService.Models;
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

        [TestMethod]
        public void IncidentHasAHome()
        {
            Home home = _visit.Incident.Home;

            string address = home.Address.Single().Value;
            address.Should().Be("221B Baker Street");
        }

        [TestMethod]
        public void IncidentHasPartsOrders()
        {
            var partsOrders = _visit.Incident.PartsOrders;

            partsOrders.Count().Should().Be(1);
            string description = partsOrders.Single().Description;
            description.Should().Be("2 inch PVC");

            partsOrders.Single().OrderReceived.Should().BeFalse();
        }

        [TestMethod]
        public void PartsAreReceived()
        {
            var partsOrder = _visit.Incident.PartsOrders.Single();

            _application.EmitMessage(partsOrder.Receive());
            partsOrder.OrderReceived.Should().BeTrue();
        }

        [TestInitialize]
        public void Initialize()
        {
            _application = new Application<Technician>();
            var technician = new Technician(Guid.NewGuid());
            _application.Load(technician);

            Guid homeId = CreateHome();
            Guid incidentId = CreateIncident(homeId);
            CreateVisit(technician, homeId, incidentId);

            _application.EmitMessage(
                _visit.Incident.CreatePartsOrder("2 inch PVC"));
        }

        private Guid CreateHome()
        {
            Guid homeId = Guid.NewGuid();
            _application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "Home",
                Guid.Empty,
                new
                {
                    HomeId = homeId
                }));
            _application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "HomeAddress",
                homeId,
                new
                {
                    Value = "221B Baker Street"
                }));
            return homeId;
        }

        private Guid CreateIncident(Guid homeId)
        {
            Guid incidentId = Guid.NewGuid();
            _application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "Incident",
                Guid.Empty,
                new
                {
                    IncidentId = incidentId,
                    HomeId = homeId
                }));
            _application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "IncidentDescription",
                incidentId,
                new
                {
                    Value = "Garbage disposal jammed"
                }));
            return incidentId;
        }

        private void CreateVisit(
            Technician technician,
            Guid homeId,
            Guid incidentId)
        {
            _application.EmitMessage(technician.CreateVisit(
                incidentId,
                homeId,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0)));

            _visit = technician.Visits.Single();
        }
    }
}
