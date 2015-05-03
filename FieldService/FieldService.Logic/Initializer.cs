using FieldService.Models;
using RoverMob;
using RoverMob.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FieldService
{
    public static class Initializer
    {
        public static Application<Technician> LoadApplication()
        {
            string folderName = "FieldService";
            var store = new FileMessageStore(folderName);
            var queue = new FileMessageQueue(folderName);
            var bookmarkStore = new FileBookmarkStore(folderName);
            var pump = new HttpMessagePump(
                new Uri("http://host/api/distributor/", UriKind.Absolute),
                queue,
                bookmarkStore);
            var push = new NoOpPushNotificationSubscription();
            var application = new Application<Technician>(
                store, queue, pump, push);
            Guid technicianId = Guid.Parse("{EF491CEC-DEEA-46F8-A303-80B6CE468AE1}");
            application.Load(new Technician(technicianId));

            return application;
        }

        public static Application<Technician> LoadDesignModeApplication()
        {
            var application = new Application<Technician>();
            var technician = new Technician(Guid.NewGuid());
            application.Load(technician);

            Schedule(application,
                "221B Baker Street", "Garbage disposal jammed", 9, 12);
            Visit visit = Schedule(application,
                "1314 Main", "Wall switch malfunction", 13, 16);
            application.EmitMessage(visit.CreateOutcome());

            return application;
        }

        private static Visit Schedule(Application<Technician> application, string address, string description, int startHour, int endHour)
        {
            Guid homeId = CreateHome(application, address);
            Guid incidentId = CreateIncident(application,
                homeId, description);
            var visit = CreateVisit(application,
                homeId, incidentId, startHour, endHour);

            application.EmitMessage(visit.Incident.CreatePartsOrder(
                "Flange"));
            application.EmitMessage(visit.Incident.CreatePartsOrder(
                "2\" PVC"));

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
