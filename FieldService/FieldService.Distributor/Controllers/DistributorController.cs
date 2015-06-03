using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace FieldService.Distributor.Controllers
{
    public class DistributorController : RoverMob.Distributor.Controllers.DistributorController
    {
        protected DistributorController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"],
            WebConfigurationManager.AppSettings["NotificationConnectionString"])
        {
            
        }
        
    }
}