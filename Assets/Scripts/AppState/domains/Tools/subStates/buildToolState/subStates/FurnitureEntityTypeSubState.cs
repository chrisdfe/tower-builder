namespace TowerBuilder.State.Tools
{
    public partial class BuildToolState : ToolStateBase
    {
        public class FurnitureEntityTypeSubState : EntityTypeSubState
        {
            public FurnitureEntityTypeSubState(BuildToolState buildToolState) : base(buildToolState) { }
        }
    }
}
