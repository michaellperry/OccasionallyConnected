using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardBoard.Messaging
{
    public interface IPushNotificationSubscription
    {
        event MessageReceivedHandler MessageReceived;
        Task Subscribe(string topic);
    }
}
