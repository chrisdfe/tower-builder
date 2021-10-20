using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map
{
    public class MapRoomDetails
    {
        public RoomCells roomCells;
        public MapRoomBuildType roomBuildType = MapRoomBuildType.Fixed;
        public Color color;
    }
}


