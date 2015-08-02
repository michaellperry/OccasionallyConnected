using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RoverMob.Distributor.Controllers;
using System.Web.Configuration;

namespace FieldService.Distributor.Controllers
{
    [Authorize]
    public class TechnicianIdentifierController : UserIdentifierController
    {
        public TechnicianIdentifierController() : base(
            WebConfigurationManager.AppSettings["StorageConnectionString"],
            "Technician")
        {

        }

        protected override bool AuthorizeUserForGet(string requestedUserId, string userId)
        {
            return false;
        }
    }
}
