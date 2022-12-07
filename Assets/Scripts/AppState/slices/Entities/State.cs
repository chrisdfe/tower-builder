using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.ApplicationState.Entities
{
    public class State : StateSlice
    {
        public class Input
        {
            public Furnitures.State.Input Furnitures = new Furnitures.State.Input();
        }

        Furnitures.State Furnitures;

        public State(AppState appState, Input input) : base(appState)
        {
            Furnitures = new Furnitures.State(appState, input.Furnitures);
        }
    }
}