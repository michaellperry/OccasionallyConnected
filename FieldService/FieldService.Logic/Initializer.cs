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
        public static Application<Technician> LoadApplication(
            IAccessTokenProvider accessTokenProvider)
        {
            string folderName = "FieldService";
            var store = new FileMessageStore(folderName);
            var queue = new FileMessageQueue(folderName);
            var bookmarkStore = new FileBookmarkStore(folderName);
            var push = new PushNotificationSubscription(
                "occdist",
                "Endpoint=sb://occdist-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=grDRsxydhFcdxbdGkamWlcvjUOixw76jEszctZ915co=");
            var pump = new HttpMessagePump(
                new Uri("http://fieldservicedistributor.azurewebsites.net/api/distributor/", UriKind.Absolute),
                queue,
                bookmarkStore,
                accessTokenProvider,
                push);


            IUserProxy proxy = new HttpUserProxy(
                new Uri("http://fieldservicedistributor.azurewebsites.net/api/technicianidentifier/", UriKind.Absolute),
                accessTokenProvider);
            var application = new Application<Technician>(
                store, queue, pump, push, proxy);


            application.GetUserIdentifier("Technician", technicianId =>
                application.Load(new Technician(technicianId)));


            pump.Subscribe(() => application.Root == null ? null :
                application.Root.GetObjectId().ToCanonicalString());
            pump.Subscribe(() => application.Root == null ? new List<string>() :
                application.Root.Visits.Select(v => v.Incident.GetObjectId().ToCanonicalString()));
            pump.Subscribe(() => application.Root == null ? new List<string>() :
                application.Root.Visits.Select(v => v.Incident.Home.GetObjectId().ToCanonicalString()));


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
