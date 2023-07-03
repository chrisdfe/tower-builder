namespace TowerBuilder.ApplicationState.Tools.Destroy.Rooms
{
    public class State : DestroyModeStateBase
    {
        public struct Input
        {
        }

        public State(AppState appState, Input input) : base(appState) { }
    }
}