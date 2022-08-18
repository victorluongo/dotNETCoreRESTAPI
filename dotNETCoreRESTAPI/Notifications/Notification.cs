using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNETCoreRESTAPI.Notifications
{
    public class Notification
    {
        public Notification(string notificationMessage)
        {
            NotificationMessage = notificationMessage;
        }

        public string NotificationMessage { get; }
    }
}