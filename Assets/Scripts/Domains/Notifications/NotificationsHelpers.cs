using System;

namespace TowerBuilder.Domains.Notifications
{
    public static class NotificationsHelpers
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