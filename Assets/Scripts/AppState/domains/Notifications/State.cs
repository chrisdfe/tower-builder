using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;

namespace TowerBuilder.State.Notifications
{
    public class State : StateSlice
    {
        public class Input
        {
            public List<Notification> allNotifications;
        }

        public class Events
        {
            public delegate void NotificationEvent(Notification notification);
            public NotificationEvent onNotificationAdded;
            public NotificationEvent onNotificationRemoved;

            public delegate void NotificationsListUpdateEvent(List<Notification> notifications);
            public NotificationsListUpdateEvent onNotificationsListUpdated;
        }

        public List<Notification> allNotifications { get; private set; } = new List<Notification>();

        public Events events;

        public State(AppState appState, Input input) : base(appState)
        {
            events = new Events();

            if (input == null) return;
            allNotifications = input.allNotifications ?? new List<Notification>();
        }

        public void AddNotification(string message)
        {
            Notification notification = new Notification(message);
            allNotifications.Add(notification);

            if (events.onNotificationAdded != null)
            {
                events.onNotificationAdded(notification);
            }

            if (events.onNotificationsListUpdated != null)
            {
                events.onNotificationsListUpdated(allNotifications);
            }
        }

        public void AddNotifications(string[] messages)
        {
            Notification[] notifications = new List<string>(messages).Select(message => new Notification(message)).ToArray();

            allNotifications = allNotifications.Concat(notifications).ToList();

            if (events.onNotificationAdded != null)
            {
                foreach (Notification notification in notifications)
                {
                    events.onNotificationAdded(notification);
                }
            }

            if (events.onNotificationsListUpdated != null)
            {
                events.onNotificationsListUpdated(allNotifications);
            }
        }
    }
}