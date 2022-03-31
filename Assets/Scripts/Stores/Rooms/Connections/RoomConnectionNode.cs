using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.Stores.Rooms.Connections
{
    public class RoomConnectionNode
    {
        public Room room;
        public RoomEntrance roomEntrance;

        public override string ToString()
        {
            return $"room: {room.id}, roomEntrance: {roomEntrance}, {roomEntrance.cellCoordinates}";
        }

        public RoomConnectionNode(Room room, RoomEntrance roomEntrance)
        {
            this.room = room;
            this.roomEntrance = roomEntrance;
        }

        public bool Matches(RoomConnectionNode otherNode)
        {
            return room == otherNode.room && roomEntrance == otherNode.roomEntrance;
        }
    }
}


