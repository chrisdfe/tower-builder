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

            public Attributes.State.Input Attributes;

            // Relations
            public FurnitureHomeSlotOccupations.State.Input furnitureHomeSlotOccupations;

            public Behaviors.State.Input Behaviors;

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

                Attributes = new Attributes.State.Input();

                // Relations
                furnitureHomeSlotOccupations = new FurnitureHomeSlotOccupations.State.Input();

                Behaviors = new Behaviors.State.Input();

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

        public Attributes.State Attributes;

        // Relations
        public FurnitureHomeSlotOccupations.State FurnitureHomeSlotOccupations;

        // Behaviors
        public Behaviors.State Behaviors;

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
            Attributes = new Attributes.State(this, input.Attributes);

            // Relations
            FurnitureHomeSlotOccupations = new FurnitureHomeSlotOccupations.State(this, input.furnitureHomeSlotOccupations);

            // Behaviors
            Behaviors = new Behaviors.State(this, input.Behaviors);
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

            Attributes.Setup();

            FurnitureHomeSlotOccupations.Setup();

            Behaviors.Setup();

            UI.Setup();
            Tools.Setup();

            watchers.Setup();
        }

        public void Teardown() { }
    }
}
