using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;

namespace TowerBuilder.ApplicationState.Notifications
{
    using NotificationsListStateSlice = ListStateSlice<NotificationsList, Notification, State.Events>;

    public class State : NotificationsListStateSlice
    {
        public class Input
        {
            public NotificationsList notificationsList;
        }

        public new class Events : NotificationsListStateSlice.Events { }

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.notificationsList ?? new NotificationsList();
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}