using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains
{
    public class StoreRegistry
    {
        public Notifications.NotificationsStore notificationsStore = new Notifications.NotificationsStore();
        public Wallet.WalletStore walletStore = new Wallet.WalletStore();
        public Rooms.RoomStore roomStore = new Rooms.RoomStore();
        public Map.MapStore mapStore = new Map.MapStore();
    }

    public static class Registry
    {
        public static StoreRegistry storeRegistry = new StoreRegistry();
    }
}
