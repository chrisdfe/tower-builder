using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Notifications
{
    public class State
    {
        public List<Notification> notifications;

        public delegate void OnNotificationAdded(Notification newNotification);
        public OnNotificationAdded onNotificationAdded;

        public void createNotification(string message)
        {
            Notification newNotification = new Notification(message);
            notifications.Add(newNotification);

            if (onNotificationAdded != null)
            {
                onNotificationAdded(newNotification);
            }
        }
    }
}