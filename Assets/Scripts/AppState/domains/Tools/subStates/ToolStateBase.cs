using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State.Rooms;

namespace TowerBuilder.State.Tools
{
    public abstract class ToolStateBase
    {
        protected AppState appState;
        protected Tools.State parentState;

        public ToolStateBase(AppState appState, Tools.State state)
        {
            this.appState = appState;
            parentState = state;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public virtual void OnSelectionBoxUpdated(SelectionBox selectionBox) { }

        public virtual void OnCurrentSelectedRoomUpdated(Room room) { }

        public virtual void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock) { }

        public virtual void OnSelectionStart(SelectionBox selectionBox) { }

        public virtual void OnSelectionEnd(SelectionBox selectionBox) { }
    }
}