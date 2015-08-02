using FieldService.Bridge.Mapping;
using FieldService.Bridge.Utility;
using Microsoft.ServiceBus.Messaging;
using RoverMob.Messaging;
using System;
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
                "fieldservice");
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
                var message = brokeredMessage.GetBody<Message>();
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
            if (message.Type == "Outcome")
            {
                using (var connection = new SqlConnection(
                    "Data Source=.;Initial Catalog=FieldService;Integrated Security=True"))
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
            }
        }
    }
}
