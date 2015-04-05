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
            string folderName = "MyApp";
            var store = new FileMessageStore(folderName);
            var queue = new FileMessageQueue(folderName);
            var bookmarkStore = new FileBookmarkStore(folderName);
            var pump = new HttpMessagePump(
                new Uri("http://host/api/distributor/", UriKind.Absolute),
                queue,
                bookmarkStore);
            var push = new PushNotificationSubscription();
            var application = new Application<Technician>(
                store, queue, pump, push);
            application.Load(new Technician(Guid.NewGuid()));
            return application;
        }

        public static Application<Technician> LoadDesignModeApplication()
        {
            var application = new Application<Technician>();
            var technician = new Technician(Guid.NewGuid());
            application.Load(technician);
            var visit = InitializeVisit(
                application,
                technician,
                "121B Baker Street",
                "Garbage disposal clogged");

            return application;
        }

        private static Visit InitializeVisit(Application<Technician> application, Technician technician, string homeAddress, string incidentDescription)
        {
            var homeId = Guid.NewGuid();
            application.EmitMessage(Message.CreateMessage(
                homeId.ToCanonicalString(),
                "HomeAddress",
                homeId,
                new
                {
                    Value = homeAddress
                }));

            var incidentId = Guid.NewGuid();
            application.EmitMessage(Message.CreateMessage(
                incidentId.ToCanonicalString(),
                "IncidentDescription",
                incidentId,
                new
                {
                    Value = incidentDescription
                }));

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
            return technician.Visits.Single(v => v.GetObjectId() == visitId);
        }
    }
}
