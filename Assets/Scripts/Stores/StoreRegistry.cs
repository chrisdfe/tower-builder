namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public struct Input
        {
            public Notifications.State.Input notifications;
            public Time.State.Input time;
            public Wallet.State.Input wallet;
            public Rooms.State.Input rooms;
            public Routes.State.Input routes;
            public Residents.State.Input residents;
            public UI.State.Input UI;
        }

        public Notifications.State Notifications = new Notifications.State();
        public Time.State Time = new Time.State();
        public Wallet.State Wallet = new Wallet.State();
        public Rooms.State Rooms = new Rooms.State();
        public Routes.State Routes = new Routes.State();
        public Residents.State Residents = new Residents.State();
        // TODO - are behaviors state? They should maybe go elsewhere
        public FurnitureBehaviors.State FurnitureBehaviors = new FurnitureBehaviors.State();
        public ResidentBehaviors.State ResidentBehaviors = new ResidentBehaviors.State();
        public UI.State UI = new UI.State();

        // public static void FromTemplate(StoreRegistry storeRegistry)
        // {
        //     return new StoreRegistry()
        //     {
        //         Wallet = new Wallet.State()
        //         {
        //             balance = 3
        //         }
        //     };
        // }
        // }
    }

    public static class Registry
    {
        public static StoreRegistry Stores = new StoreRegistry();
    }
}
