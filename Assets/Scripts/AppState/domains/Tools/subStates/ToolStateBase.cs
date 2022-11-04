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
            Registry.appState.UI.onCurrentSelectedCellUpdated += OnCurrentSelectedCellUpdated;
            Registry.appState.UI.onCurrentSelectedRoomUpdated += OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.onCurrentSelectedRoomBlockUpdated += OnCurrentSelectedRoomBlockUpdated;
        }

        public virtual void Teardown()
        {
            Registry.appState.UI.onCurrentSelectedCellUpdated -= OnCurrentSelectedCellUpdated;
            Registry.appState.UI.onCurrentSelectedRoomUpdated -= OnCurrentSelectedRoomUpdated;
            Registry.appState.UI.onCurrentSelectedRoomBlockUpdated -= OnCurrentSelectedRoomBlockUpdated;
        }

        public virtual void OnCurrentSelectedCellUpdated(CellCoordinates currentSelectedCell) { }

        public virtual void OnCurrentSelectedRoomUpdated(Room room) { }

        public virtual void OnCurrentSelectedRoomBlockUpdated(RoomCells roomBlock) { }
    }
}