using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class InspectToolState : ToolStateBase
    {
        public Room currentSelectedRoom;
        public Room currentInspectedRoom;
        public delegate void RoomEvent(Room room);
        public RoomEvent onCurrentSelectedRoomUpdated;
        public RoomEvent onCurrentInspectedRoomUpdated;

        public InspectToolState(MapUI.State state) : base(state) { }

        public override void Reset()
        {
            AttemptToInspectRoom(null);
            SelectRoom(null);
        }

        public void AttemptToInspectRoom(Room roomToInspect)
        {
            currentInspectedRoom = roomToInspect;

            if (onCurrentInspectedRoomUpdated != null)
            {
                onCurrentInspectedRoomUpdated(currentInspectedRoom);
            }
        }

        public override void OnCurrentSelectedCellSet()
        {
            CellCoordinates currentSelectedCell = parentState.currentSelectedCell;

            if (currentSelectedCell == null)
            {
                return;
            }

            Room currentRoom = MapStore.Helpers.FindRoomAtCell(currentSelectedCell, Registry.Stores.Map.mapRooms);
            SelectRoom(currentRoom);
        }

        void SelectRoom(Room room)
        {
            this.currentSelectedRoom = room;

            if (onCurrentSelectedRoomUpdated != null)
            {
                onCurrentSelectedRoomUpdated(this.currentSelectedRoom);
            }
        }
    }
}