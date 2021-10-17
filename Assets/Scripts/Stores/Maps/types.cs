using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public struct CellFloorCoordinates
    {
        public int x;
        public int z;
    }

    public struct CellCoordinates
    {
        public int x;
        public int z;
        public int floor;
    };

    public struct CellCoordinates2D
    {
        public int x;
        public int z;
    }

    public class RoomShape : List<CellCoordinates> { };

    public class RoomShapeMap : Dictionary<RoomKey, RoomShape> { };

    public class RoomCellsMap : Dictionary<string, List<CellCoordinates>> { };

    public class RoomGroup : List<string> { };

    public class RoomGroupMap : Dictionary<string, RoomGroup> { };

    public struct MapState
    {
        public RoomCellsMap roomCellsMap;
        public RoomGroupMap roomGroupMap;
    }
}
