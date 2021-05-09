using System;

namespace TowerBuilder.Domains.Notifications
{
    public static class NotificationsMutations
    {
        public static void createNotification(StoreRegistry storeRegistry, NotificationInput notificationInput)
        {
            Notification newNotification = NotificationsHelpers.createNotification(notificationInput);

            NotificationsState previousState = storeRegistry.notificationsStore.state;
            storeRegistry.notificationsStore.state.notificationsMap.Add(newNotification.id, newNotification);
            NotificationsState state = storeRegistry.notificationsStore.state;

            if (NotificationsEvents.onNotificationsStateUpdated != null)
            {
                NotificationsEvents.onNotificationsStateUpdated(new NotificationsStateEventPayload()
                {
                    previousState = previousState,
                    state = state
                });
            }
        }
    }
}