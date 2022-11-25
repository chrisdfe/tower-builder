using TowerBuilder.DataTypes.TransportationItems;

namespace TowerBuilder.ApplicationState.TransportationItems
{
    public class State : StateSlice
    {
        public class Input
        {
            public TransportationItemsList transportationItemsList;
        }

        public class Events { }

        public class Queries { }

        public TransportationItemsList transportationItemsList { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            transportationItemsList = input.transportationItemsList ?? new TransportationItemsList();
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}