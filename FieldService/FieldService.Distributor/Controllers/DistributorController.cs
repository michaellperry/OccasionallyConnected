using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Threading.Tasks;
using RoverMob.Protocol;
using RoverMob.Distributor;

namespace FieldService.Distributor.Controllers
{
    public class DistributorController : RoverMob.Distributor.Controllers.DistributorController
    {
        protected DistributorController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"],
            WebConfigurationManager.AppSettings["NotificationConnectionString"])
        {
            
        }

        protected override async Task<bool> AuthorizeUserForGet(string topic, string userId)
        {
            Guid technicianId = GetUserIdentifier("Technician", userId);
            return topic == technicianId.ToCanonicalString() || VisitsWithNoOutcome(userId)
                .Any(m => m.VisitId == topic || m.HomeId == topic);
        }

        protected override async Task<bool> AuthorizeUserForPost(string topic, string userId)
        {
            return VisitsWithNoOutcome(userId)
                .Any(m => m.VisitId == topic);
        }

        private IEnumerable<dynamic> VisitsWithNoOutcome(string userId)
        {
            Guid technicianId = GetUserIdentifier("Technician", userId);
            return GetMessagesInTopic(
                technicianId.ToCanonicalString(),
                "Visit",
                "Outcome",
                "visit");
        }
    }
}