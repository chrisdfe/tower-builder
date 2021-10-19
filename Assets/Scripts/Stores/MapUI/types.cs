using System;
using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.MapUI
{
    public struct MapUIState
    {
        public ToolState toolState;
        public RoomKey selectedRoomKey;
        public CellCoordinates currentSelectedTile;
        public MapRoomBlueprint currentBlueprint;
        public MapRoomRotation currentBlueprintRotation;
        public int currentFocusFloor;
    }
}