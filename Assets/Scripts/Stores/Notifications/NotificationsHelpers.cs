using System;

namespace TowerBuilder.Stores.Notifications
{
    public partial class NotificationsStore
    {
        public static class Helpers
        {
            public static Notification createNotification(NotificationInput notificationInput)
            {
                string id = System.Guid.NewGuid().ToString();
                return new Notification()
                {
                    id = id,
                    message = notificationInput.message
                };
            }
        }
    }
}