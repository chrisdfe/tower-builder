using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State.Rooms;

namespace TowerBuilder.State.Tools
{
    public abstract class ToolStateBase
    {
        protected Tools.State parentState;

        public ToolStateBase(Tools.State state)
        {
            parentState = state;
        }

        public virtual void Setup()
        {
            Registry.appState.UI.events.onSelectionStart += OnSelectionStart;
            Registry.appState.UI.events.onSelectionEnd += OnSelectionEnd;
            Registry.appState.UI.events.onSelectionBoxUpdated += OnSelectionBoxUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;
        }

        public virtual void Teardown()
        {
            Registry.appState.UI.events.onSelectionStart -= OnSelectionStart;
            Registry.appState.UI.events.onSelectionEnd -= OnSelectionEnd;
            Registry.appState.UI.events.onSelectionBoxUpdated -= OnSelectionBoxUpdated;

            Registry.appState.UI.events.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.events.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;
        }

        protected virtual void OnSelectionBoxUpdated(SelectionBox selectionBox) { }

        protected virtual void OnCurrentSelectedRoomUpdated(Room room) { }

        protected virtual void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock) { }

        protected virtual void OnSelectionStart(SelectionBox selectionBox) { }

        protected virtual void OnSelectionEnd(SelectionBox selectionBox) { }
    }
}