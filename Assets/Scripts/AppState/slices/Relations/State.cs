using TowerBuilder.DataTypes.Relations;

namespace TowerBuilder.ApplicationState.Relations
{
    public class State : StateSlice
    {
        public class Input
        {
            public FurnitureHomeSlotOccupations.State.Input FurnitureHomeSlotOccupations;
        }

        public FurnitureHomeSlotOccupations.State FurnitureHomeSlotOccupations;

        public State(AppState appState, Input input) : base(appState)
        {
            FurnitureHomeSlotOccupations = new FurnitureHomeSlotOccupations.State(appState, input.FurnitureHomeSlotOccupations);
        }

        public override void Setup()
        {
            base.Setup();

            FurnitureHomeSlotOccupations.Setup();
        }
    }
}