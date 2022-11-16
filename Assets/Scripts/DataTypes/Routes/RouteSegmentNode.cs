using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
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