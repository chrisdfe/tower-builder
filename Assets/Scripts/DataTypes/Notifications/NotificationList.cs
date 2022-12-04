using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;

namespace TowerBuilder.DataTypes.Notifications
{
    public class NotificationsList : ListWrapper<Notification>
    {
        public NotificationsList() : base() { }
        public NotificationsList(Notification notification) : base(notification) { }
        public NotificationsList(List<Notification> notifications) : base(notifications) { }
        public NotificationsList(NotificationsList notificationsList) : base(notificationsList) { }
    }
}