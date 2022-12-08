using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.ApplicationState.Entities
{
    public class State : StateSlice
    {
        public class Input
        {
            public Furnitures.State.Input Furnitures = new Furnitures.State.Input();
            public Residents.State.Input Residents = new Residents.State.Input();

            public Rooms.State.Input Rooms;
            public TransportationItems.State.Input TransportationItems;
            public Vehicles.State.Input Vehicles;
            public Journeys.State.Input Journeys;
            public Freight.State.Input Freight;

            public Input()
            {
                Rooms = new Rooms.State.Input();
                Furnitures = new Furnitures.State.Input();
                Residents = new Residents.State.Input();
                TransportationItems = new TransportationItems.State.Input();
                Vehicles = new Vehicles.State.Input();
                Journeys = new Journeys.State.Input();
                Freight = new Freight.State.Input();
            }
        }

        public Rooms.State Rooms { get; }
        public Furnitures.State Furnitures { get; }
        public Residents.State Residents { get; }
        public TransportationItems.State TransportationItems { get; }
        public Vehicles.State Vehicles { get; }
        public Journeys.State Journeys { get; }
        public Freight.State Freight { get; }

        public State(AppState appState, Input input) : base(appState)
        {
            Rooms = new Rooms.State(appState, input.Rooms);
            Furnitures = new Furnitures.State(appState, input.Furnitures);
            Residents = new Residents.State(appState, input.Residents);
            TransportationItems = new TransportationItems.State(appState, input.TransportationItems);
            Vehicles = new Vehicles.State(appState, input.Vehicles);
            Journeys = new Journeys.State(appState, input.Journeys);
            Freight = new Freight.State(appState, input.Freight);
        }
    }
}