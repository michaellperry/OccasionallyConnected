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
            application.Load(new Technician(Guid.NewGuid()));
            return application;
        }

        public static Application<Technician> LoadDesignModeApplication()
        {
            var application = new Application<Technician>();
            var technician = new Technician(Guid.NewGuid());
            application.Load(technician);

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
                    Value = "221B Baker Street"
                }));

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
                    Value = "Garbage disposal jammed"
                }));

            application.EmitMessage(technician.CreateVisit(
                incidentId,
                homeId,
                new DateTime(2015, 5, 1, 9, 0, 0),
                new DateTime(2015, 5, 1, 12, 0, 0)));
            application.EmitMessage(technician.CreateVisit(
                incidentId,
                homeId,
                new DateTime(2015, 5, 1, 13, 0, 0),
                new DateTime(2015, 5, 1, 16, 0, 0)));

            var visit = technician.Visits.ElementAt(1);
            application.EmitMessage(visit.CreateOutcome());

            return application;
        }
    }
}
