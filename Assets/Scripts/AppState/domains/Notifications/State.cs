using System.Collections;
using System.Collections.Generic;

using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;

namespace TowerBuilder.State.Notifications
{
    public class State
    {
        public class Input
        {
            public List<Notification> allNotifications;
        }

        public List<Notification> allNotifications { get; private set; } = new List<Notification>();

        public delegate void NotificationsListEvent(List<Notification> notifications, Notification notification);
        public NotificationsListEvent onNotificationAdded;
        public NotificationsListEvent onNotificationRemoved;

        public delegate void NotificationsListUpdateEvent(List<Notification> notifications);
        public NotificationsListUpdateEvent onNotificationsUpdated;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            if (input == null) return;
            allNotifications = input.allNotifications ?? new List<Notification>();
        }

        public void createNotification(string message)
        {
            allNotifications.Add(new Notification(message));
        }
    }
}