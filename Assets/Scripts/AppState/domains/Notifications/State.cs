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

        public ResourceList<Notification> allNotifications { get; private set; } = new ResourceList<Notification>();

        public State() : this(new Input()) { }

        public State(Input input)
        {
            if (input == null) return;
            allNotifications.Set(input.allNotifications ?? new List<Notification>());
        }

        public void createNotification(string message)
        {
            allNotifications.Add(new Notification(message));
        }
    }
}