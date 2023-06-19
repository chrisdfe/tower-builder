using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;

namespace TowerBuilder.ApplicationState.Tools
{
    public abstract class ToolStateBase
    {
        protected AppState appState;
        protected Tools.State toolsState;

        public ToolStateBase(AppState appState, Tools.State toolsState)
        {
            this.appState = appState;
            this.toolsState = toolsState;
        }

        public virtual void Setup() { }

        public virtual void Teardown() { }

        public virtual void OnSelectionBoxUpdated(SelectionBox selectionBox) { }

        public virtual void OnCurrentSelectedEntityListUpdated(ListWrapper<Entity> entityList) { }

        public virtual void OnSelectedEntityBlocksUpdated(CellCoordinatesBlockList selectedBlocksList) { }

        public virtual void OnCurrentSelectedRoomUpdated(Room room) { }

        public virtual void OnSelectionStart(SelectionBox selectionBox) { }

        public virtual void OnSelectionEnd(SelectionBox selectionBox) { }

        public virtual void OnSelectionBoxReset(SelectionBox selectionBox) { }

        public virtual void OnSecondaryActionStart() { }

        public virtual void OnSecondaryActionEnd() { }
    }
}