using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains
{
    public class StoreRegistry
    {
        public Rooms.RoomStore roomStore = new Rooms.RoomStore();
        public Map.MapStore mapStore = new Map.MapStore();
        public Notifications.NotificationsStore notificationsStore = new Notifications.NotificationsStore();
    }

    public static class Registry
    {
        public static StoreRegistry storeRegistry = new StoreRegistry();
    }
}
