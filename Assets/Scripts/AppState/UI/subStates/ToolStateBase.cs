using System.Collections.Generic;

using TowerBuilder.State.Rooms;

namespace TowerBuilder.State.UI
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

        public virtual void OnCurrentSelectedRoomUpdated(Room room) { }

        public virtual void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock) { }
    }
}