using TowerBuilder.DataTypes.Freights;

namespace TowerBuilder.ApplicationState.Entities.Freight.FreightItems
{
    using FreightItemListStateSlice = ListStateSlice<FreightItemList, FreightItem, State.Events>;

    public class State : FreightItemListStateSlice
    {
        public class Input
        {
            public FreightItemList freightItemList;
        }

        public new class Events : FreightItemListStateSlice.Events { }

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
            list = input.freightItemList ?? new FreightItemList();

            queries = new Queries(appState, this);

            Setup();
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}