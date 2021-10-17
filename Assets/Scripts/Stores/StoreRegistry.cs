using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Notifications;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Time;
using TowerBuilder.Stores.Wallet;

namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public NotificationsStore notificationsStore = new NotificationsStore();
        public TimeStore timeStore = new TimeStore();
        public WalletStore walletStore = new WalletStore();
        public RoomStore roomStore = new RoomStore();
        public MapStore mapStore = new MapStore();
        public MapUIStore mapUIStore = new MapUIStore();
    }

    public static class Registry
    {
        public static StoreRegistry storeRegistry = new StoreRegistry();
    }
}
