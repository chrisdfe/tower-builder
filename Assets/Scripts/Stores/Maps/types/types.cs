using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public enum MapRoomBuildType
    {
        // e.g offices, condos
        Fixed,
        // e.g Lobbies, hallways
        Flexible
    }

    public enum MapRoomRotation
    {
        Down,
        Right,
        Up,
        Left
    }

    public struct MapState
    {
        public List<MapRoom> mapRooms;
    };
}


