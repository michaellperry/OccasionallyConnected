using CardBoard.Messaging;
using CardBoard.Protocol;
using Microsoft.WindowsAzure.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;

namespace CardBoard.Notification
{
    public class PushNotificationSubscription : IPushNotificationSubscription
    {
        public event MessageReceivedHandler MessageReceived;

        public async Task Subscribe(string topic)
        {
            var channel = await PushNotificationChannelManager
                .CreatePushNotificationChannelForApplicationAsync();
            channel.PushNotificationReceived += channel_PushNotificationReceived;

            var hub = new NotificationHub("occdist", "Endpoint=sb://occdist-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=+unDBkvenHMZKDwK8ZFZywiemEbFTC5Q64Op1J0TqZw=");
            await hub.RegisterNativeAsync(channel.Uri,
                new string[] { topic });
        }

        void channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            if (args.NotificationType == PushNotificationType.Raw)
            {
                var message = JsonConvert.DeserializeObject<MessageMemento>(
                    args.RawNotification.Content);

                if (MessageReceived != null)
                    MessageReceived(CardBoard.Messaging.Message.FromMemento(
                        message));
            }
        }
    }
}
