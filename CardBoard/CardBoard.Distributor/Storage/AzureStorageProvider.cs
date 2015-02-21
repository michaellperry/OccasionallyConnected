using CardBoard.Protocol;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Configuration;

namespace CardBoard.Distributor.Storage
{
    public class AzureStorageProvider
    {
        public void WriteMessage(string topic, MessageMemento message)
        {
            var messageTable = OpenMessageTable();

            var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmssfffffff");
            var entity = new MessageEntity(topic, timestamp);
            entity.Hash = message.Hash;
            entity.MessageType = message.MessageType;
            entity.Predecessors = JsonConvert.SerializeObject(message.Predecessors);
            entity.ObjectId = message.ObjectId;
            entity.Body = JsonConvert.SerializeObject(message.Body);

            var insert = TableOperation.Insert(entity);
            messageTable.Execute(insert);
        }

        private static CloudTable OpenMessageTable()
        {
            var storageAccount = CloudStorageAccount.Parse(
                WebConfigurationManager.AppSettings["StorageConnectionString"]);

            var tableClient = storageAccount.CreateCloudTableClient();
            var messageTable = tableClient.GetTableReference("Message");
            messageTable.CreateIfNotExists();

            return messageTable;
        }
    }
}
