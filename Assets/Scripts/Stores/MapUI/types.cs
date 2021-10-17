using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.MapUI
{
    public enum ToolState
    {
        None,
        Build,
        Inspect,
        Destroy
    }

    // public enum OverlayState
    // {
    //     None,
    //     Coordinates,
    // }

    public struct MapUIState
    {
        public ToolState toolState;
        public RoomKey selectedRoomKey;
        public int currentFocusFloor;
    }
}