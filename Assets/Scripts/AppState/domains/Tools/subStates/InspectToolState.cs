using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.UI;
using UnityEngine;

namespace TowerBuilder.State.Tools
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

        public InspectToolState(Tools.State state, Input input) : base(state)
        {
            currentInspectedRoom = input.currentInspectedRoom ?? null;
        }

        public override void Teardown()
        {
            base.Teardown();
            InspectRoom(null);
        }

        public void InspectCurrentSelectedRoom()
        {
            InspectRoom(Registry.appState.UI.currentSelectedRoom);
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