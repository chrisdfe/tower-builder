namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public Notifications.State Notifications = new Notifications.State();
        public Time.State Time = new Time.State();
        public Wallet.State Wallet = new Wallet.State();
        public Map.State Map = new Map.State();
        public MapUI.State MapUI = new MapUI.State();
        public Routes.State Routes = new Routes.State();
        public Residents.State Residents = new Residents.State();
    }

    public static class Registry
    {
        public static StoreRegistry Stores = new StoreRegistry();
    }
}
