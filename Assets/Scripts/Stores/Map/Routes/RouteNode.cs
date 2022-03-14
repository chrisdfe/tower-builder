using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Routes
{
    public class RouteNode
    {
        public CellCoordinates cellCoordinates;
        public Room room;

        public RouteNode(CellCoordinates cellCoordinates, Room room)
        {
            this.cellCoordinates = cellCoordinates;
            this.room = room;
        }
    }
}