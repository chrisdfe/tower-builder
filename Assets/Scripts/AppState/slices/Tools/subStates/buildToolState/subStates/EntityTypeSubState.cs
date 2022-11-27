namespace TowerBuilder.ApplicationState.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class EntityTypeSubState
        {
            protected BuildToolState buildToolState;

            public EntityTypeSubState(BuildToolState buildToolState)
            {
                this.buildToolState = buildToolState;
            }

            public virtual void Setup() { }
            public virtual void Teardown() { }
            public virtual void StartBuild() { }
            public virtual void EndBuild() { }
            public virtual void OnSelectionBoxUpdated() { }
        }
    }
}
