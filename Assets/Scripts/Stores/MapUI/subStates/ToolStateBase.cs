using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;

namespace TowerBuilder.Stores.MapUI
{
    public abstract class ToolStateBase
    {
        protected MapUI.State parentState;

        public ToolStateBase(MapUI.State state)
        {
            parentState = state;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }
    }
}