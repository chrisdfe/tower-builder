using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using TowerBuilder.Stores.MapUI;
using UnityEngine;

namespace TowerBuilder.Stores.MapUI
{
    public class InspectToolState : ToolStateBase
    {
        public Room currentInspectedRoom;
        public delegate void RoomEvent(Room room);
        public RoomEvent onCurrentInspectedRoomUpdated;

        public InspectToolState(MapUI.State state) : base(state) { }

        public override void Teardown()
        {
            InspectRoom(null);
        }

        public void InspectCurrentSelectedRoom()
        {
            InspectRoom(parentState.currentSelectedRoom);
        }

        public void InspectRoom(Room room)
        {
            this.currentInspectedRoom = room;

            if (onCurrentInspectedRoomUpdated != null)
            {
                onCurrentInspectedRoomUpdated(room);
            }
        }
    }
}