namespace TowerBuilder.State
{
    public class AppState
    {
        public class Input
        {
            public Notifications.State.Input notifications;
            public Time.State.Input time;
            public Wallet.State.Input wallet;

            public Buildings.State.Input buildings;
            public Furnitures.State.Input furnitures;

            public Rooms.State.Input rooms;

            public Routes.State.Input routes;
            public Residents.State.Input residents;
            public UI.State.Input ui;

            public Input()
            {
                notifications = new Notifications.State.Input();
                time = new Time.State.Input();
                wallet = new Wallet.State.Input();

                buildings = new Buildings.State.Input();
                furnitures = new Furnitures.State.Input();
                rooms = new Rooms.State.Input();

                routes = new Routes.State.Input();
                residents = new Residents.State.Input();
                ui = new UI.State.Input();
            }
        }

        public Notifications.State Notifications = new Notifications.State();
        public Time.State Time = new Time.State();
        public Wallet.State Wallet = new Wallet.State();

        public Buildings.State buildings = new Buildings.State();
        public Furnitures.State Furnitures = new Furnitures.State();
        public Rooms.State Rooms = new Rooms.State();

        public Routes.State Routes = new Routes.State();
        public Residents.State Residents = new Residents.State();

        public FurnitureBehaviors.State FurnitureBehaviors = new FurnitureBehaviors.State();
        public ResidentBehaviors.State ResidentBehaviors = new ResidentBehaviors.State();

        public UI.State UI = new UI.State();

        public AppState(Input input)
        {
            Notifications = new Notifications.State(input.notifications);
            Time = new Time.State(input.time);
            Wallet = new Wallet.State(input.wallet);

            buildings = new Buildings.State(input.buildings);
            Furnitures = new Furnitures.State(input.furnitures);
            Rooms = new Rooms.State(input.rooms);

            Routes = new Routes.State(input.routes);
            Residents = new Residents.State(input.residents);
            UI = new UI.State(input.ui);
        }

        public AppState() : this(new Input()) { }
    }
}
