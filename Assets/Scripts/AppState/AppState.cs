using TowerBuilder.DataTypes;

namespace TowerBuilder.ApplicationState
{
    public class AppState : ISetupable
    {
        public class Input
        {
            public Notifications.State.Input notifications;
            public Time.State.Input time;
            public Wallet.State.Input wallet;
            public Journeys.State.Input Journeys;
            public Vehicles.State.Input Vehicles;

            public Entities.State.Input Entities;

            // Attributes
            public ResidentAttributesWrappers.State.Input residentAttributesWrappers;
            public VehicleAttributesWrappers.State.Input vehicleAttributesWrappers;

            // Relations
            public FurnitureHomeSlotOccupations.State.Input furnitureHomeSlotOccupations;

            // Behaviors
            public ResidentBehaviors.State.Input residentBehaviors;
            public FurnitureBehaviors.State.Input furnitureBehaviors;

            public UI.State.Input ui;
            public Tools.State.Input tools;

            public Input()
            {
                notifications = new Notifications.State.Input();
                time = new Time.State.Input();
                wallet = new Wallet.State.Input();
                Journeys = new Journeys.State.Input();
                Vehicles = new Vehicles.State.Input();

                Entities = new Entities.State.Input();

                // Attributes
                residentAttributesWrappers = new ResidentAttributesWrappers.State.Input();
                vehicleAttributesWrappers = new VehicleAttributesWrappers.State.Input();

                // Relations
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
        public Journeys.State Journeys;
        public Vehicles.State Vehicles;

        public Entities.State Entities;

        // Attributes
        public ResidentAttributesWrappers.State ResidentAttributesWrappers;
        public VehicleAttributesWrappers.State VehicleAttributesWrappers;

        // Relations
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
            Journeys = new Journeys.State(this, input.Journeys);
            Vehicles = new Vehicles.State(this, input.Vehicles);

            Entities = new Entities.State(this, input.Entities);

            // Attributes
            ResidentAttributesWrappers = new ResidentAttributesWrappers.State(this, input.residentAttributesWrappers);
            VehicleAttributesWrappers = new VehicleAttributesWrappers.State(this, input.vehicleAttributesWrappers);

            // Relations
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

        public void Setup()
        {
            Notifications.Setup();
            Time.Setup();
            Wallet.Setup();
            Journeys.Setup();
            Vehicles.Setup();

            Entities.Setup();

            ResidentAttributesWrappers.Setup();
            VehicleAttributesWrappers.Setup();

            FurnitureHomeSlotOccupations.Setup();

            FurnitureBehaviors.Setup();
            ResidentBehaviors.Setup();

            UI.Setup();
            Tools.Setup();

            watchers.Setup();
        }

        public void Teardown() { }
    }
}
