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
            var visit = InitializeVisit(application, technician);
            return application;
        }

        private static Visit InitializeVisit(Application<Technician> application, Technician technician)
        {
            var visitId = Guid.NewGuid();
            var message = Message.CreateMessage(
                technician.GetObjectId().ToCanonicalString(),
                "Visit",
                technician.GetObjectId(),
                new
                {
                    VisitId = visitId
                });
            application.EmitMessage(message);
            return technician.Visits.Single(v => v.GetObjectId() == visitId);
        }
    }
}
