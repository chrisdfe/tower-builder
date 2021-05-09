using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Notifications
{
    public struct NotificationInput
    {
        public string message;
    }

    public struct Notification
    {
        public string id;
        public string message;
    }

    public struct NotificationsState
    {
        public Dictionary<string, Notification> notificationsMap;
    }
}