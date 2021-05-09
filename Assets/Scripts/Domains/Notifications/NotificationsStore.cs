using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Notifications
{
    public class NotificationsStore : StoreBase<NotificationsState>
    {
        public NotificationsStore()
        {
            state.notificationsMap = new Dictionary<string, Notification>();
        }
    }
}
