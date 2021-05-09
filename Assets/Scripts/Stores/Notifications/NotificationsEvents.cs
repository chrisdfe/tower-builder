using System;

namespace TowerBuilder.Stores.Notifications
{
    public partial class NotificationsStore
    {
        public struct NotificationsStateEventPayload
        {
            public NotificationsState state;
            public NotificationsState previousState;
        };

        public static class Events
        {
            public delegate void OnNotificationsStateUpdated(NotificationsStateEventPayload payload);
            public static OnNotificationsStateUpdated onNotificationsStateUpdated;
        }
    }
}