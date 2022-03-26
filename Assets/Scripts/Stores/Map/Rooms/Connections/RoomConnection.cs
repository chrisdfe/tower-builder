using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Connections
{
    public class RoomConnection
    {
        public RoomConnectionNode nodeA;
        public RoomConnectionNode nodeB;

        public RoomConnection(Room roomA, RoomEntrance roomAEntrance, Room roomB, RoomEntrance roomBEntrance)
        {
            this.nodeA = new RoomConnectionNode(roomA, roomAEntrance);
            this.nodeB = new RoomConnectionNode(roomB, roomBEntrance);
        }

        public bool ContainsRooms(Room roomA, Room roomB)
        {
            return ContainsRoom(roomA) && ContainsRoom(roomB);
        }

        public bool ContainsRoom(Room room)
        {
            return room == nodeA.room || room == nodeB.room;
        }

        public bool ContainsRoomEntrance(RoomEntrance roomEntrance)
        {
            return roomEntrance == nodeA.roomEntrance || roomEntrance == nodeB.roomEntrance;
        }

        public RoomConnectionNode GetConnectionFor(Room room)
        {
            if (nodeA.room == room)
            {
                return nodeA;
            }

            if (nodeB.room == room)
            {
                return nodeB;
            }

            return null;
        }
    }
}


