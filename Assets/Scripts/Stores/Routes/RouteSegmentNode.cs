using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public class RouteSegmentNode
    {
        public CellCoordinates cellCoordinates { get; private set; }
        public Room room { get; private set; }
        // public RoomEntrance roomEntrance { get; private set; }

        public RouteSegmentNode(CellCoordinates cellCoordinates, Room room)
        {
            this.cellCoordinates = cellCoordinates;
            this.room = room;
        }

        // public RouteSegmentNode(CellCoordinates cellCoordinates, Room room, RoomEntrance roomEntrance)
        // {
        //     this.cellCoordinates = cellCoordinates;
        //     this.room = room;
        //     this.roomEntrance = roomEntrance;
        // }

        public RouteSegmentNode Clone()
        {
            return new RouteSegmentNode(cellCoordinates.Clone(), room);
        }
    }
}