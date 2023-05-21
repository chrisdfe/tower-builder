using TowerBuilder.DataTypes.Entities;

namespace TowerBuilder.ApplicationState.EntityGroups
{
    public class State : StateSlice
    {
        public class Input
        {

        }

        public State(AppState appState, Input input) : base(appState)
        {
        }
    }
}