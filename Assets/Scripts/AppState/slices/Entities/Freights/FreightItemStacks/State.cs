using TowerBuilder.DataTypes.Entities.Freights;

namespace TowerBuilder.ApplicationState.Entities.Freight.FreightItemStacks
{
    using FreightItemStackListStateSlice = ListStateSlice<FreightItemStackList, FreightItemStack, State.Events>;

    public class State : FreightItemStackListStateSlice
    {
        public class Input
        {
            public FreightItemStackList freightItemStackList;
        }

        public new class Events : FreightItemStackListStateSlice.Events { }

        public class Queries
        {
            AppState appState;
            State state;

            public Queries(AppState appState, State state)
            {
                this.appState = appState;
                this.state = state;
            }
        }

        public Queries queries;

        public State(AppState appState, Input input) : base(appState)
        {
            list = input.freightItemStackList ?? new FreightItemStackList();

            queries = new Queries(appState, this);

            Setup();
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}