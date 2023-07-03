namespace TowerBuilder.ApplicationState.Tools.Routes
{
    public class State : ToolStateBase
    {
        public struct Input
        {
            public ClickState? currentClickState;
        }

        public enum ClickState
        {
            None,
            RouteStart,
            RouteEnd,
        }

        public ClickState currentClickState { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {
            currentClickState = input.currentClickState ?? ClickState.None;
        }

        public void SetClickState(ClickState clickState)
        {
            this.currentClickState = clickState;
        }
    }
}