using System.Collections.Generic;
using TowerBuilder.ApplicationState.Entities.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Rooms;

namespace TowerBuilder.ApplicationState.Tools
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

        public virtual void OnCurrentSelectedRoomBlockUpdated(CellCoordinatesBlock roomBlock) { }

        public virtual void OnCurrentSelectedEntityListUpdated(EntityList entityList) { }

        public virtual void OnSelectionStart(SelectionBox selectionBox) { }

        public virtual void OnSelectionEnd(SelectionBox selectionBox) { }
    }
}