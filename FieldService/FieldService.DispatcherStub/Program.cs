using FieldService.Models;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService.DispatcherStub
{
    class Program
    {
        static void Main(string[] args)
        {
            var pump = new HttpMessagePump(
                new Uri("http://fieldservicedistributor.azurewebsites.net/api/distributor/", UriKind.Absolute),
                new NoOpMessageQueue(),
                new NoOpBookmarkStore());
            var application = new Application<Technician>(
                new NoOpMessageStore(),
                new NoOpMessageQueue(),
                pump,
                new NoOpPushNotificationSubscription(),
                new NoOpUserProxy());

            // This is the technician ID that was created for
            // my login.
            application.Load(new Technician(Guid.Parse(
                "{13a5e507-6376-4c99-85d4-e04ac4bfa1b7}")));

            // Schedule a visit for this technician.
            Console.WriteLine("Scheduling a visit");
            Visit visit = Schedule(application,
                "1314 Main", "Wall switch malfunction", 13, 16);

            Console.WriteLine("Press enter...");
            Console.ReadLine();

            // Create a new parts order for the visit.
            Console.WriteLine("Adding a parts order");
            application.EmitMessage(visit.Incident.CreatePartsOrder(
                "16' electrical cable"));

            Console.WriteLine("Press enter...");
            Console.ReadLine();
        }

        private static Visit Schedule(Application<Technician> application, string address, string description, int startHour, int endHour)
        {
            Guid homeId = CreateHome(application, address);
            Guid incidentId = CreateIncident(application,
                homeId, description);
            var visit = CreateVisit(application,
                homeId, incidentId, startHour, endHour);

            application.EmitMessage(visit.Incident.CreatePartsOrder(
                "Switch box"));

            var partsOrder = visit.Incident.PartsOrders.First();
            application.EmitMessage(partsOrder.Receive());

            return visit;
        }

        private static Guid CreateHome(
            Application<Technician> application,
            string address)
        {
            Guid homeId = Guid.NewGuid();
            application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "Home",
                Guid.Empty,
                new
                {
                    HomeId = homeId
                }));
            application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "HomeAddress",
                homeId,
                new
                {
                    Value = address
                }));
            return homeId;
        }

        private static Guid CreateIncident(
            Application<Technician> application,
            Guid homeId,
            string description)
        {
            var incidentId = Guid.NewGuid();
            application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "Incident",
                Guid.Empty,
                new
                {
                    IncidentId = incidentId,
                    HomeId = homeId
                }));
            application.EmitMessage(Message.CreateMessage(
                string.Empty,
                "IncidentDescription",
                incidentId,
                new
                {
                    Value = description
                }));
            return incidentId;
        }

        private static Visit CreateVisit(
            Application<Technician> application,
            Guid homeId,
            Guid incidentId,
            int startHour,
            int endHour)
        {
            Message createVisitMessage = application.Root.CreateVisit(
                incidentId,
                homeId,
                new DateTime(2015, 5, 1, startHour, 0, 0),
                new DateTime(2015, 5, 1, endHour, 0, 0));
            application.EmitMessage(createVisitMessage);

            return application.Root.Visits.Single(v =>
                v.VisitHash == createVisitMessage.Hash);
        }
    }
}
