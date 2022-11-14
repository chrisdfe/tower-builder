using UnityEngine;

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
            public Rooms.State.Input rooms;

            public Furnitures.State.Input furnitures;
            public Residents.State.Input residents;

            public Routes.State.Input routes;

            public ResidentBehaviors.State.Input residentBehaviors;
            public FurnitureBehaviors.State.Input furnitureBehaviors;

            public UI.State.Input ui;
            public Tools.State.Input tools;

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

                residentBehaviors = new ResidentBehaviors.State.Input();
                furnitureBehaviors = new FurnitureBehaviors.State.Input();

                ui = new UI.State.Input();
                tools = new Tools.State.Input();
            }
        }

        public Notifications.State Notifications;
        public Time.State Time;
        public Wallet.State Wallet;

        public Buildings.State Buildings;
        public Furnitures.State Furnitures;
        public Rooms.State Rooms;

        public Routes.State Routes;
        public Residents.State Residents;

        public FurnitureBehaviors.State FurnitureBehaviors;
        public ResidentBehaviors.State ResidentBehaviors;

        public UI.State UI;
        public Tools.State Tools;

        public AppState(Input input)
        {
            Notifications = new Notifications.State(this, input.notifications);
            Time = new Time.State(this, input.time);
            Wallet = new Wallet.State(this, input.wallet);

            Buildings = new Buildings.State(this, input.buildings);
            Rooms = new Rooms.State(this, input.rooms);
            Furnitures = new Furnitures.State(this, input.furnitures);
            Residents = new Residents.State(this, input.residents);

            Routes = new Routes.State(this, input.routes);

            FurnitureBehaviors = new FurnitureBehaviors.State(this, input.furnitureBehaviors);
            ResidentBehaviors = new ResidentBehaviors.State(this, input.residentBehaviors);

            UI = new UI.State(this, input.ui);
            Tools = new Tools.State(this, input.tools);
        }

        public AppState() : this(new Input()) { }
    }
}
