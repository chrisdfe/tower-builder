namespace TowerBuilder.Stores
{
    public class StoreRegistry
    {
        public class Input
        {
            public Notifications.State.Input notifications;
            public Time.State.Input time;
            public Wallet.State.Input wallet;
            public Rooms.State.Input rooms;
            public Routes.State.Input routes;
            public Residents.State.Input residents;
            public UI.State.Input ui;

            public Input()
            {
                notifications = new Notifications.State.Input();
                time = new Time.State.Input();
                wallet = new Wallet.State.Input();
                rooms = new Rooms.State.Input();
                routes = new Routes.State.Input();
                residents = new Residents.State.Input();
                ui = new UI.State.Input();
            }
        }

        public Notifications.State Notifications = new Notifications.State();
        public Time.State Time = new Time.State();
        public Wallet.State Wallet = new Wallet.State();
        public Rooms.State Rooms = new Rooms.State();
        public Routes.State Routes = new Routes.State();
        public Residents.State Residents = new Residents.State();
        // TODO - are behaviors state? These should maybe go elsewhere
        public FurnitureBehaviors.State FurnitureBehaviors = new FurnitureBehaviors.State();
        public ResidentBehaviors.State ResidentBehaviors = new ResidentBehaviors.State();
        public UI.State UI = new UI.State();


        public StoreRegistry(Input input)
        {
            Notifications = new Notifications.State(input.notifications);
            Time = new Time.State(input.time);
            Wallet = new Wallet.State(input.wallet);
            Rooms = new Rooms.State(input.rooms);
            Routes = new Routes.State(input.routes);
            Residents = new Residents.State(input.residents);
            UI = new UI.State(input.ui);
        }

        public StoreRegistry() : this(new Input()) { }
    }

    public static class Registry
    {
        public static StoreRegistry Stores = new StoreRegistry();
    }
}
