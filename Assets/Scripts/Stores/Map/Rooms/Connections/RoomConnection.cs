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
        public RoomEntrance roomAEntrance;
        public Room roomB;
        public RoomEntrance roomBEntrance;

        public RoomConnection(Room roomA, RoomEntrance roomAEntrance, Room roomB, RoomEntrance roomBEntrance)
        {
            this.roomA = roomA;
            this.roomB = roomB;
            this.roomAEntrance = roomAEntrance;
            this.roomBEntrance = roomBEntrance;
        }

        public bool ContainsRooms(Room roomA, Room roomB)
        {
            return ContainsRoom(roomA) && ContainsRoom(roomB);
        }

        public bool ContainsRoom(Room room)
        {
            return room == roomA || room == roomB;
        }

        public bool ContainsRoomEntrance(RoomEntrance roomEntrance)
        {
            return roomEntrance == roomAEntrance || roomEntrance == roomBEntrance;
        }

        public (Room room, RoomEntrance roomEntrance) GetConnectionFor(Room room)
        {
            if (room == roomA)
            {
                return (roomB, roomBEntrance);
            }

            if (room == roomB)
            {
                return (roomA, roomAEntrance);
            }

            return (null, null);
        }
    }
}


