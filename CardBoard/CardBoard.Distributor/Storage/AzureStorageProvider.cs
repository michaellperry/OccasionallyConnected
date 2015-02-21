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

        public PageMemento ReadMessages(string topic, string bookmark)
        {
            var messageTable = OpenMessageTable();

            var query = new TableQuery<MessageEntity>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition(
                        "PartitionKey", QueryComparisons.Equal, topic),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition(
                        "RowKey", QueryComparisons.GreaterThan, bookmark)));

            var messages = messageTable.ExecuteQuery(query);

            return new PageMemento
            {
                Bookmark = messages.Select(m => m.RowKey).Max() ??
                    bookmark,
                Messages = messages.Select(m => new MessageMemento
                {
                    Hash = m.Hash,
                    Topic = m.PartitionKey,
                    MessageType = m.MessageType,
                    Predecessors = JsonConvert.DeserializeObject<List<string>>(
                        m.Predecessors),
                    ObjectId = m.ObjectId,
                    Body = JsonConvert.DeserializeObject<ExpandoObject>(m.Body)
                }).ToList()
            };
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
