using System;

namespace TowerBuilder.Stores.Notifications
{
    public partial class NotificationsStore
    {
        public static class Mutations
        {
            public static void createNotification(StoreRegistry storeRegistry, NotificationInput notificationInput)
            {
                Notification newNotification = NotificationsStore.Helpers.createNotification(notificationInput);

                NotificationsState previousState = storeRegistry.notificationsStore.state;
                storeRegistry.notificationsStore.state.notificationsMap.Add(newNotification.id, newNotification);
                NotificationsState state = storeRegistry.notificationsStore.state;

                if (NotificationsStore.Events.onNotificationsStateUpdated != null)
                {
                    NotificationsStore.Events.onNotificationsStateUpdated(new NotificationsStateEventPayload()
                    {
                        previousState = previousState,
                        state = state
                    });
                }
            }

            public static void createNotification(StoreRegistry storeRegistry, string message)
            {
                NotificationInput notificationInput = new NotificationInput()
                {
                    message = message
                };

                createNotification(storeRegistry, notificationInput);
            }
        }
    }
}