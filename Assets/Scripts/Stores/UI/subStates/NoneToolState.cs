using System.Collections.Generic;


namespace TowerBuilder.Stores.UI
{
    public class NoneToolState : ToolStateBase
    {
        public struct Input { }

        public NoneToolState(UI.State state, Input input) : base(state) { }
    }
}