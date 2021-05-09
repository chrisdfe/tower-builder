using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Notifications
{
    public partial class NotificationsStore
    {
        public static class Selectors
        {
            public static List<string> getNotificationsList(StoreRegistry storeRegistry)
            {
                NotificationsStore notificationsStore = storeRegistry.notificationsStore;
                return notificationsStore.state.notificationsMap.Values.ToList().Select(notification =>
                {
                    return notification.message;
                }).ToList();
            }
        }
    }
}