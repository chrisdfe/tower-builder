using TowerBuilder.DataTypes.Entities.Freights;

namespace TowerBuilder.ApplicationState.Entities.Freight
{
    public class State : StateSlice
    {
        public class Input
        {
            public FreightItemStackGroups.State freightItemStackGroups;
            public FreightItemStacks.State freightItemStacks;
            public FreightItems.State freightItems;
        }

        public FreightItemStackGroups.State FreightItemStackGroups { get; private set; }
        public FreightItemStacks.State FreightItemStacks { get; private set; }
        public FreightItems.State FreightItems { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            FreightItems = input.freightItems ?? new FreightItems.State(appState);
            FreightItemStacks = input.freightItemStacks ?? new FreightItemStacks.State(appState);
            FreightItemStackGroups = input.freightItemStackGroups ?? new FreightItemStackGroups.State(appState);
        }

        public State(AppState appState) : this(appState, new Input()) { }
    }
}