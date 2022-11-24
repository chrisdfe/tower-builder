namespace TowerBuilder.ApplicationState
{
    public class WatcherBase
    {
        protected AppState appState;

        public WatcherBase(AppState appState)
        {
            this.appState = appState;
        }

        public virtual void Setup() { }
        public virtual void Teardown() { }
    }
}
