using CardBoard.Distributor.Models;
using CardBoard.Distributor.Storage;
using CardBoard.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CardBoard.Distributor.Controllers
{
    public class DistributorController : ApiController
    {
        private AzureStorageProvider _storage = new AzureStorageProvider();

        public void Post(string topic, [FromBody]MessageMemento message)
        {
            _storage.WriteMessage(topic, message);
        }

        public PageMemento Get(string topic, string bookmark)
        {
            throw new NotImplementedException();
        }
    }
}