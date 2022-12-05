using TowerBuilder.ApplicationState.Freight.FreightItems;

namespace TowerBuilder.ApplicationState.Freight
{
    public class State : StateSlice
    {
        public class Input
        {
            public FreightItems.State freightItems;
        }

        FreightItems.State freightItems;

        public State(AppState appState, Input input) : base(appState)
        {
            freightItems = input.freightItems ?? new FreightItems.State(appState);
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}