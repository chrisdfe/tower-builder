namespace TowerBuilder.ApplicationState
{
    public class AppState
    {
        public class Input
        {
            public Notifications.State.Input notifications;
            public Time.State.Input time;
            public Wallet.State.Input wallet;

            public Rooms.State.Input rooms;
            public Furnitures.State.Input furnitures;
            public Residents.State.Input residents;
            public TransportationItems.State.Input transportationItems;
            public Vehicles.State.Input vehicles;
            public Journeys.State.Input journeys;

            public FurnitureHomeSlotOccupations.State.Input furnitureHomeSlotOccupations;

            public ResidentBehaviors.State.Input residentBehaviors;
            public FurnitureBehaviors.State.Input furnitureBehaviors;

            public VehicleAttributeGroups.State.Input vehicleAttributeGroups;

            public UI.State.Input ui;
            public Tools.State.Input tools;

            public Input()
            {
                notifications = new Notifications.State.Input();
                time = new Time.State.Input();
                wallet = new Wallet.State.Input();

                // Base models
                rooms = new Rooms.State.Input();
                furnitures = new Furnitures.State.Input();
                residents = new Residents.State.Input();
                transportationItems = new TransportationItems.State.Input();
                vehicles = new Vehicles.State.Input();
                journeys = new Journeys.State.Input();

                // Attributes
                vehicleAttributeGroups = new VehicleAttributeGroups.State.Input();

                // Associations
                furnitureHomeSlotOccupations = new FurnitureHomeSlotOccupations.State.Input();

                // Behaviors
                residentBehaviors = new ResidentBehaviors.State.Input();
                furnitureBehaviors = new FurnitureBehaviors.State.Input();

                // UI
                ui = new UI.State.Input();
                tools = new Tools.State.Input();
            }
        }

        public Notifications.State Notifications;
        public Time.State Time;
        public Wallet.State Wallet;

        // Models
        public Rooms.State Rooms;
        public Furnitures.State Furnitures;
        public Residents.State Residents;
        public TransportationItems.State TransportationItems;
        public Vehicles.State Vehicles;
        public Journeys.State Journeys;

        // Attributes
        public VehicleAttributeGroups.State VehicleAttributeGroups;

        // Associations
        public FurnitureHomeSlotOccupations.State FurnitureHomeSlotOccupations;

        // Behaviors
        public FurnitureBehaviors.State FurnitureBehaviors;
        public ResidentBehaviors.State ResidentBehaviors;

        public UI.State UI;
        public Tools.State Tools;

        class Watchers
        {
            AppState appState;
            BedCreationWatcher bedCreationWatcher;

            public Watchers(AppState appState)
            {
                this.appState = appState;

                bedCreationWatcher = new BedCreationWatcher(appState);

                Setup();
            }

            public void Setup()
            {
                bedCreationWatcher.Setup();
            }
        }

        Watchers watchers;

        public AppState(Input input)
        {
            Notifications = new Notifications.State(this, input.notifications);
            Time = new Time.State(this, input.time);
            Wallet = new Wallet.State(this, input.wallet);

            // Models
            Rooms = new Rooms.State(this, input.rooms);
            Furnitures = new Furnitures.State(this, input.furnitures);
            Residents = new Residents.State(this, input.residents);
            TransportationItems = new TransportationItems.State(this, input.transportationItems);
            Vehicles = new Vehicles.State(this, input.vehicles);
            Journeys = new Journeys.State(this, input.journeys);

            // Attributes
            VehicleAttributeGroups = new VehicleAttributeGroups.State(this, input.vehicleAttributeGroups);

            // Associations
            FurnitureHomeSlotOccupations = new FurnitureHomeSlotOccupations.State(this, input.furnitureHomeSlotOccupations);

            // Behaviors
            FurnitureBehaviors = new FurnitureBehaviors.State(this, input.furnitureBehaviors);
            ResidentBehaviors = new ResidentBehaviors.State(this, input.residentBehaviors);


            // UI
            UI = new UI.State(this, input.ui);
            Tools = new Tools.State(this, input.tools);

            watchers = new Watchers(this);
        }

        public AppState() : this(new Input()) { }
    }
}
