using CardBoard.Distributor.Models;
using CardBoard.Distributor.Storage;
using CardBoard.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CardBoard.Distributor.Notification;
using System.Threading.Tasks;

namespace CardBoard.Distributor.Controllers
{
    public class DistributorController : ApiController
    {
        private AzureStorageProvider _storage = new AzureStorageProvider();
        private AzurePushNotificationProvider _pushNotification = new AzurePushNotificationProvider();

        public async Task Post(string topic, [FromBody]MessageMemento message)
        {
            _storage.WriteMessage(topic, message);
            await _pushNotification.SendNotificationAsync(topic, message);
        }

        public PageMemento Get(string topic, string bookmark)
        {
            return _storage.ReadMessages(topic, bookmark);
        }
    }
}