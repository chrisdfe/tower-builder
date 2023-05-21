using TowerBuilder.DataTypes;

namespace TowerBuilder.ApplicationState
{
    public class AppState : ISetupable
    {
        public class Input
        {
            public Notifications.State.Input Notifications;
            public Time.State.Input Time;
            public Wallet.State.Input Wallet;
            public Journeys.State.Input Journeys;

            public Entities.State.Input Entities;
            public EntityGroups.State.Input EntityGroups;
            public Attributes.State.Input Attributes;
            public Relations.State.Input Relations;
            public Behaviors.State.Input Behaviors;

            public UI.State.Input ui;
            public Tools.State.Input tools;

            public Input()
            {
                Notifications = new Notifications.State.Input();
                Time = new Time.State.Input();
                Wallet = new Wallet.State.Input();
                Journeys = new Journeys.State.Input();

                Entities = new Entities.State.Input();
                EntityGroups = new EntityGroups.State.Input();
                Attributes = new Attributes.State.Input();
                Relations = new Relations.State.Input();
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

        public Entities.State Entities;
        public EntityGroups.State EntityGroups;
        public Attributes.State Attributes;
        public Relations.State Relations;
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
            Notifications = new Notifications.State(this, input.Notifications);
            Time = new Time.State(this, input.Time);
            Wallet = new Wallet.State(this, input.Wallet);
            Journeys = new Journeys.State(this, input.Journeys);

            Entities = new Entities.State(this, input.Entities);
            EntityGroups = new EntityGroups.State(this, input.EntityGroups);

            Attributes = new Attributes.State(this, input.Attributes);
            Relations = new Relations.State(this, input.Relations);
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

            Entities.Setup();
            EntityGroups.Setup();
            Attributes.Setup();
            Relations.Setup();
            Behaviors.Setup();

            UI.Setup();
            Tools.Setup();

            watchers.Setup();
        }

        public void Teardown() { }
    }
}
