namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public Notifications.State Notifications = new Notifications.State();
        public Time.State Time = new Time.State();
        public Wallet.State Wallet = new Wallet.State();
        public Rooms.State Rooms = new Rooms.State();
        public Routes.State Routes = new Routes.State();
        public Residents.State Residents = new Residents.State();
        public FurnitureBehaviors.State FurnitureBehaviors = new FurnitureBehaviors.State();
        public ResidentBehaviors.State ResidentBehaviors = new ResidentBehaviors.State();
        public UI.State UI = new UI.State();
    }

    public static class Registry
    {
        public static StoreRegistry Stores = new StoreRegistry();
    }
}
