using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class DestroyToolState : ToolStateBase
    {
        public Room currentSelectedRoom;
        public delegate void CurrentSelectedRoomEvent(Room currentSelectedRoom);
        public CurrentSelectedRoomEvent onCurrentSelectedRoomUpdated;

        public DestroyToolState(MapUI.State state) : base(state) { }

        public void AttemptToDestroyRoom(Room roomToDestroy)
        {
            if (roomToDestroy == null)
            {
                return;
            }

            Registry.Stores.Map.DestroyRoom(roomToDestroy);
        }

        public override void OnCurrentSelectedCellSet()
        {
            CellCoordinates currentSelectedCell = parentState.currentSelectedCell;

            if (currentSelectedCell == null)
            {
                return;
            }

            Room currentRoom = MapStore.Helpers.FindRoomAtCell(currentSelectedCell, Registry.Stores.Map.mapRooms);

            this.currentSelectedRoom = currentRoom;

            if (onCurrentSelectedRoomUpdated != null)
            {
                onCurrentSelectedRoomUpdated(this.currentSelectedRoom);
            }
        }
    }
}