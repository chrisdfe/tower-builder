using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public struct MapCoordinates
    {
        public int x;
        public int y;
    };

    public class RoomShape : List<MapCoordinates> { };

    public class RoomShapeMap : Dictionary<RoomKey, RoomShape> { };

    public class RoomCellsMap : Dictionary<string, MapCoordinates[]> { };

    public class RoomGroup : List<string> { };

    public class RoomGroupMap : Dictionary<string, RoomGroup> { };

    public struct MapState
    {
        public RoomCellsMap roomCellsMap;
        public RoomGroupMap roomGroupMap;
    }
}
