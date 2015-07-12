using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Threading.Tasks;
using RoverMob.Protocol;
using RoverMob.Distributor;
using System.Web.Http;

namespace FieldService.Distributor.Controllers
{
    //[Authorize]
    // Temporarily disabled so that the dispatcher stub can work.
    public class DistributorController : RoverMob.Distributor.Controllers.DistributorController
    {
        protected DistributorController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"],
            WebConfigurationManager.AppSettings["NotificationConnectionString"])
        {
            
        }

        protected override async Task<bool> AuthorizeUserForGet(string topic, string userId)
        {
            if (userId == null)
                return true;

            var technicianId = GetUserIdentifier("Technician", userId)
                .ToCanonicalString();

            if (topic == technicianId)
                return true;

            var visits = GetMessagesInTopic(
                technicianId,
                "Visit",
                "Outcome",
                "visit");

            if (visits.Any(v =>
                GetCanonicalString(v.IncidentId) == topic ||
                GetCanonicalString(v.HomeId) == topic))
                return true;

            return false;
        }

        protected override async Task<bool> AuthorizeUserForPost(string topic, string userId)
        {
            if (userId == null)
                return true;

            var technicianId = GetUserIdentifier("Technician", userId)
                .ToCanonicalString();

            var visits = GetMessagesInTopic(
                technicianId,
                "Visit",
                "Outcome",
                "visit");

            if (visits.Any(v =>
                GetCanonicalString(v.IncidentId) == topic))
                return true;

            return false;
        }

        private string GetCanonicalString(string id)
        {
            return Guid.Parse(id).ToCanonicalString();
        }
    }
}