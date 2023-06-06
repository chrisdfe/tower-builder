using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Notifications;

namespace TowerBuilder.ApplicationState.Notifications
{
    using NotificationsListStateSlice = ListStateSlice<Notification, State.Events>;

    public class State : NotificationsListStateSlice
    {
        public class Input
        {
            public ListWrapper<Notification> notificationsList;
        }

        public new class Events : NotificationsListStateSlice.Events { }

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.notificationsList ?? new ListWrapper<Notification>();
        }

        public State(AppState appState) : this(appState, new Input()) { }

        public void Add(ListWrapper<ValidationError> validationErrors)
        {
            Add(
                new ListWrapper<Notification>(
                    validationErrors.items
                        .Select(error => new Notification(error.message))
                        .ToList()
                )
            );
        }
    }
}