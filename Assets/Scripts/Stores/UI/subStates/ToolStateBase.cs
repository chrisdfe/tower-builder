using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.UI
{
    public abstract class ToolStateBase
    {
        protected UI.State parentState;

        public ToolStateBase(UI.State state)
        {
            parentState = state;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public virtual void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell) { }
    }
}