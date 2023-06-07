using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Freights;

namespace TowerBuilder.ApplicationState.Entities.Freight
{
    using FreightItemListStateSlice = EntityStateSlice<FreightItem, State.Events>;

    public class State : FreightItemListStateSlice
    {
        public class Input
        {
            public ListWrapper<FreightItem> freightItemList;
        }

        public new class Events : FreightItemListStateSlice.Events { }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}