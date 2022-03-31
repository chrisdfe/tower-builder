using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.Stores.UI
{
    public class InspectToolState : ToolStateBase
    {
        public Room currentInspectedRoom;
        public delegate void RoomEvent(Room room);
        public RoomEvent onCurrentInspectedRoomUpdated;

        public InspectToolState(UI.State state) : base(state) { }

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