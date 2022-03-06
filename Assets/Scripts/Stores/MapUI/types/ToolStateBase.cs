using System.Collections.Generic;
using TowerBuilder.Stores.Map;

namespace TowerBuilder.Stores.MapUI
{
    public abstract class ToolStateBase
    {
        protected MapUI.State parentState;

        public ToolStateBase(MapUI.State state)
        {
            parentState = state;
        }

        public virtual void Reset() { }

        public virtual void OnCurrentSelectedCellSet() { }
    }
}