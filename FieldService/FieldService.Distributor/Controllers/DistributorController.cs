﻿using System;
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
                v.IncidentId == topic ||
                v.HomeId == topic))
                return true;

            return false;
        }

        protected override async Task<bool> AuthorizeUserForPost(string topic, string userId)
        {
            var technicianId = GetUserIdentifier("Technician", userId)
                .ToCanonicalString();

            var visits = GetMessagesInTopic(
                technicianId,
                "Visit",
                "Outcome",
                "visit");

            if (visits.Any(v =>
                v.IncidentId == topic))
                return true;

            return false;
        }
    }
}