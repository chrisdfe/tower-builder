using TowerBuilder.DataTypes.Entities.Freights;

namespace TowerBuilder.ApplicationState.Entities.Freight.FreightItemStackGroups
{
    using FreightItemStackGroupListStateSlice = ListStateSlice<FreightItemStackGroupList, FreightItemStackGroup, State.Events>;

    public class State : FreightItemStackGroupListStateSlice
    {
        public class Input
        {
            public FreightItemStackGroupList list;
        }

        public new class Events : FreightItemStackGroupListStateSlice.Events { }

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
            list = input.list ?? new FreightItemStackGroupList();

            queries = new Queries(appState, this);

            Setup();
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}