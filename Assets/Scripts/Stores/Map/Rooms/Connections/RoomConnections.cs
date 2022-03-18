using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Connections
{
    public class RoomConnections
    {
        public List<RoomConnection> connections { get; private set; } = new List<RoomConnection>();

        public RoomConnections() { }

        public RoomConnections(List<RoomConnection> connections)
        {
            this.connections = connections;
        }

        public void Add(RoomConnections roomConnections)
        {
            this.connections = this.connections.Concat(roomConnections.connections).ToList();
        }

        public void Add(RoomConnection roomConnection)
        {
            this.connections.Add(roomConnection);
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

        public RoomConnections FindConnectionsForRoom(Room room)
        {
            return new RoomConnections(
                connections.FindAll(roomConnection => roomConnection.ContainsRoom(room))
            );
        }

        public RoomConnection FindConnectionForRoomEntrance(RoomEntrance roomEntrance)
        {
            return connections.Find(roomConnection => roomConnection.ContainsRoomEntrance(roomEntrance));
        }

        public RoomConnections SearchForNewConnectionsToRoom(RoomList roomList, Room targetRoom)
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

                if (roomAtCell == null)
                {
                    continue;
                }

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

            return new RoomConnections(result);
        }
    }
}


