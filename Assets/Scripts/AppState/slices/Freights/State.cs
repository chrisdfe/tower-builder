using TowerBuilder.ApplicationState.Freight.FreightItems;

namespace TowerBuilder.ApplicationState.Freight
{
    public class State : StateSlice
    {
        public class Input
        {
            FreightItems.State freightItems;
        }

        FreightItems.State freightItems;

        public State(AppState appState, Input input) : base(appState)
        {

        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}