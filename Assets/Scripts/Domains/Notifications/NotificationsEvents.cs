using System;

namespace TowerBuilder.Domains.Notifications
{
    public struct NotificationsStateEventPayload
    {
        public NotificationsState state;
        public NotificationsState previousState;
    };

    public static class NotificationsEvents
    {
        public delegate void OnNotificationsStateUpdated(NotificationsStateEventPayload payload);
        public static OnNotificationsStateUpdated onNotificationsStateUpdated;
    }
}