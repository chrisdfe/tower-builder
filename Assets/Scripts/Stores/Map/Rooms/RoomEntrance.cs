using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;

namespace TowerBuilder.Stores.Map.Rooms
{
    public class RoomEntrance
    {
        public Room room;
        public RoomCell roomCell;
        public RoomEntrancePosition position;
        public CellCoordinates cellCoordinates;
        public RoomEntrance connection;
    }
}


