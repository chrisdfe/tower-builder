using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Notifications
{
    public partial class NotificationsStore : StoreBase<NotificationsState>
    {
        public NotificationsStore()
        {
            state.notificationsMap = new Dictionary<string, Notification>();
        }
    }
}
