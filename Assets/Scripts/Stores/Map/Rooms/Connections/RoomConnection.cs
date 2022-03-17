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
        public Room roomA;
        public Room roomB;

        public RoomConnection(Room roomA, Room roomB)
        {
            this.roomA = roomA;
            this.roomB = roomB;
        }

        public bool ContainsRooms(Room roomA, Room roomB)
        {
            return ContainsRoom(roomA) && ContainsRoom(roomB);
        }

        public bool ContainsRoom(Room room)
        {
            return room == roomA || room == roomB;
        }

        public Room GetConnectionFor(Room room)
        {
            if (room == roomA)
            {
                return roomB;
            }

            if (room == roomB)
            {
                return roomA;
            }

            return null;
        }
    }
}


