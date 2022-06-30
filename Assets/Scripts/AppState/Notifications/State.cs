using System.Collections;
using System.Collections.Generic;

using TowerBuilder.State.Notifications;

namespace TowerBuilder.State.Notifications
{
    public class State
    {
        public class Input
        {
            public List<Notification> notifications;
        }

        public List<Notification> notifications { get; private set; }
        public delegate void OnNotificationAdded(Notification newNotification);
        public OnNotificationAdded onNotificationAdded;

        public State() : this(new Input()) { }

        public State(Input input)
        {
            if (input == null) return;
            notifications = input.notifications ?? new List<Notification>();
        }

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