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
        public List<CellCoordinates> roomCells = RoomCells.CreateRectangularRoom(0, 0);
        public RoomResizability roomResizability = RoomResizability.Inflexible();
        public Color color = Color.white;
    }
}


