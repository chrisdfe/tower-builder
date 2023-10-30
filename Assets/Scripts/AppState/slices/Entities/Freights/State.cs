using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Freights;

namespace TowerBuilder.ApplicationState.Entities.Freight
{
    public class State : EntityStateSliceBase
    {
        public class Input
        {
            public ListWrapper<FreightItem> freightItemList;
        }

        public State(AppState appState, Input input) : base(appState)
        {
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}