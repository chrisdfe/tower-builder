using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Connections
{
    public class RoomConnections
    {
        public List<RoomConnection> connections = new List<RoomConnection>();

        // public void AddConnection(Room roomA, Room roomB)
        // {
        //     RoomConnection newRoomConnection = new RoomConnection(roomA, roomB);
        //     connections.Add(newRoomConnection);
        // }

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

        public List<RoomConnection> FindConnectionsForRoom(Room room)
        {
            return connections.FindAll(roomConnection => roomConnection.ContainsRoom(room));
        }

        public RoomConnection FindConnectionForRoomEntrance(RoomEntrance roomEntrance)
        {
            return connections.Find(roomConnection => roomConnection.ContainsRoomEntrance(roomEntrance));
        }

        public List<RoomConnection> SearchForNewConnectionsToRoom(RoomList roomList, Room targetRoom)
        {
            // Debug.Log("searching for connections to room " + targetRoom.id);
            List<RoomConnection> result = new List<RoomConnection>();

            foreach (RoomEntrance targetRoomEntrance in targetRoom.entrances)
            {
                CellCoordinates targetCell;
                if (targetRoomEntrance.position == RoomEntrancePosition.Left)
                {
                    // Debug.Log("search to the cell to the left");
                    CellCoordinates relativeCellToTheLeft = targetRoomEntrance.cellCoordinates.Add(new CellCoordinates(-1, 0));
                    CellCoordinates cellToTheLeft =
                        targetRoom.roomCells.GetBottomLeftCoordinates().Add(relativeCellToTheLeft);
                    targetCell = cellToTheLeft;
                }
                else
                {
                    // RoomEntrancePosition.right
                    // Debug.Log("search to the cell to the right");
                    CellCoordinates relativeCellToTheLeft = targetRoomEntrance.cellCoordinates.Add(new CellCoordinates(1, 0));
                    CellCoordinates cellToTheRight =
                        targetRoom.roomCells.GetBottomLeftCoordinates().Add(relativeCellToTheLeft);
                    targetCell = cellToTheRight;
                }

                Room roomAtCell = roomList.FindRoomAtCell(targetCell);
                // Debug.Log(roomAtCell);
                if (roomAtCell == null)
                {
                    continue;
                }

                // Debug.Log("room is not null.");

                foreach (RoomEntrance roomAtCellEntrance in roomAtCell.entrances)
                {
                    CellCoordinates absoluteEntranceCoordinates =
                        roomAtCell.roomCells
                            .GetBottomLeftCoordinates()
                            .Add(roomAtCellEntrance.cellCoordinates);

                    if (absoluteEntranceCoordinates.Matches(targetCell))
                    {
                        // Debug.Log("it matches!");
                        // Debug.Log(absoluteEntranceCoordinates);
                        RoomConnection newRoomConnection = new RoomConnection(targetRoom, roomAtCellEntrance, roomAtCell, roomAtCellEntrance);
                        result.Add(newRoomConnection);
                    }
                }
            }

            return result;
        }
    }
}


