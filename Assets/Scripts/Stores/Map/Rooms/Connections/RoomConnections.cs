using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Blueprints;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Connections
{
    public class RoomConnections
    {
        List<RoomConnection> connections = new List<RoomConnection>();

        public void AddConnection(Room roomA, Room roomB)
        {
            RoomConnection newRoomConnection = new RoomConnection(roomA, roomB);
            connections.Add(newRoomConnection);
        }

        public void RemoveConnectionsForRoom(Room room)
        {
            connections = connections.Where(
                roomConnection => !roomConnection.ContainsRoom(room)
            ).ToList();
        }

        public void RemoveConnectionBetween(Room roomA, Room roomB)
        {
            connections = connections.Where(
                roomConnection => !roomConnection.ContainsRooms(roomA, roomB)
            ).ToList();
        }

        public List<RoomConnection> SearchForNewConnectionsToRoom(RoomList roomList, Room room)
        {
            List<RoomConnection> result = new List<RoomConnection>();
            return result;
        }
    }
}


