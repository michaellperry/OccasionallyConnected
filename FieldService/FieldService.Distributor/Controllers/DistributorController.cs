using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Threading.Tasks;

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
            return true;
        }

        protected override async Task<bool> AuthorizeUserForPost(string topic, string userId)
        {
            return true;
        }
    }
}