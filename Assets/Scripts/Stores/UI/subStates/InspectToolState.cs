using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.UI;
using UnityEngine;

namespace TowerBuilder.Stores.UI
{
    public class InspectToolState : ToolStateBase
    {
        public struct Input
        {
            public Room currentInspectedRoom;
        }

        public Room currentInspectedRoom;
        public delegate void RoomEvent(Room room);
        public RoomEvent onCurrentInspectedRoomUpdated;

        public InspectToolState(UI.State state, Input input) : base(state)
        {
            currentInspectedRoom = input.currentInspectedRoom ?? null;
        }

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