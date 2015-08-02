using FieldService.Bridge.Mapping;
using FieldService.Bridge.Utility;
using Microsoft.ServiceBus.Messaging;
using RoverMob.Messaging;
using RoverMob.Protocol;
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FieldService.Bridge.Queueing
{
    class FieldServiceSubscriber
    {
        private readonly MessageIdMap _messageIdMap;

        private QueueClient _queueClient;
        
        public FieldServiceSubscriber(MessageIdMap messageIdMap)
        {
            _messageIdMap = messageIdMap;
        }

        public void Start()
        {
            _queueClient = QueueClient.CreateFromConnectionString(
                "Endpoint=sb://mlpfieldservicemsg-ns.servicebus.windows.net/;SharedAccessKeyName=EnterpriseApp;SharedAccessKey=yu2sFV++PrfqEjkvWHqo5rlaW1zeplBfR1Qp91afJ2w=",
                "fieldservicemessages");
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                AutoRenewTimeout = TimeSpan.FromMinutes(2)
            };
            _queueClient.OnMessageAsync(HandleBrokeredMessageAsync, options);
        }

        public void Stop()
        {
        }

        private async Task HandleBrokeredMessageAsync(BrokeredMessage brokeredMessage)
        {
            try
            {
                var messageMemento = brokeredMessage.GetBody<MessageMemento>();
                var message = Message.FromMemento(messageMemento);
                Console.WriteLine("Received message: {0}", message.Type);

                await HandleMessageAsync(message);

                await brokeredMessage.CompleteAsync();
                return;
            }
            catch (Exception)
            {
                // Leave the catch block so we can call an async method.
            }
            await brokeredMessage.AbandonAsync();
        }

        private async Task HandleMessageAsync(Message message)
        {
            if (message.Type == "Visit")
                await WithConnection(c => HandleVisit(message, c));
            if (message.Type == "Outcome")
                await WithConnection(c => HandleOutcome(message, c));
        }

        private async Task HandleVisit(Message message, DbConnection connection)
        {
            string incidentIdStr = message.Body.IncidentId;
            DateTime dateOfVisit = message.Body.StartTime;

            var incidentId = Guid.Parse(incidentIdStr);
            int incidentFk = await _messageIdMap.GetDatabaseId(
                "Visit", incidentId);
            string visitHashStr = Convert.ToBase64String(message.Hash.Code);

            await connection.ExecuteAsync(
                "exec VisitSP @p1, @p2, @p3, ''",
                incidentFk, visitHashStr, dateOfVisit);
        }

        private async Task HandleOutcome(Message message, DbConnection connection)
        {
            string incidentIdStr = message.Body.IncidentId;
            var incidentId = Guid.Parse(incidentIdStr);
            int incidentFk = await _messageIdMap.GetDatabaseId(
                "Incident", incidentId);

            MessageHash visitHash = message.GetPredecessors("visit")
                .FirstOrDefault();
            string base64Hash = Convert.ToBase64String(visitHash.Code);

            await connection.ExecuteAsync(
                "exec Outcome @p1, @p2", incidentFk, base64Hash);
        }

        private static async Task WithConnection(Func<DbConnection, Task> handler)
        {
            using (var connection = new SqlConnection(
                "Data Source=.;Initial Catalog=FieldService;Integrated Security=True"))
            {
                connection.Open();

                await handler(connection);
            }
        }
    }
}
