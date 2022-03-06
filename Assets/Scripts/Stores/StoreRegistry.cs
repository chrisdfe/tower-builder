using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using TowerBuilder.Stores.Notifications;
using TowerBuilder.Stores.Time;
using TowerBuilder.Stores.Wallet;

namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public Notifications.State Notifications = new Notifications.State();
        public Time.State Time = new Time.State();
        public Wallet.State Wallet = new Wallet.State();
        public Map.State Map = new Map.State();
        public MapUI.State MapUI = new MapUI.State();
    }

    public static class Registry
    {
        public static StoreRegistry Stores = new StoreRegistry();
    }
}
