using System.Collections.Generic;

namespace TowerBuilder.State.Tools
{
    public class NoneToolState : ToolStateBase
    {
        public struct Input { }

        public NoneToolState(Tools.State state, Input input) : base(state) { }
    }
}