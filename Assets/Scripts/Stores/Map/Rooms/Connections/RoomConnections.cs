using System.Collections.Generic;
using System.Linq;

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

        public List<RoomConnection> SearchForNewConnectionsToRoom(RoomList roomList, Room targetRoom)
        {
            List<RoomConnection> result = new List<RoomConnection>();

            // TODO
            foreach (Room room in roomList.rooms)
            {
                foreach (RoomCell roomCell in room.roomCells.cells)
                {

                }
            }

            return result;
        }
    }
}


