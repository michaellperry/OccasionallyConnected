using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CardBoard.Protocol;
using System.Web.Configuration;
using Microsoft.ServiceBus.Notifications;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CardBoard.Distributor.Notification
{
    public class AzurePushNotificationProvider
    {
        public async Task SendNotificationAsync(string topic, MessageMemento message)
        {
            var connectionString = WebConfigurationManager.AppSettings["NotificationConnectionString"];
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(
                    connectionString, "occdist");

            var json = JsonConvert.SerializeObject(message);
            var notification = new WindowsNotification(json,
                new Dictionary<string, string> {
				{"X-WNS-Type", "wns/raw"}
			});
            await hub.SendNotificationAsync(notification, topic);
        }
    }
}
