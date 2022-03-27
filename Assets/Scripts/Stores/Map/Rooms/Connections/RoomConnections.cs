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

        public void Add(Room roomA, RoomEntrance roomAEntrance, Room roomB, RoomEntrance roomBEntrance)
        {
            this.connections.Add(new RoomConnection(roomA, roomAEntrance, roomB, roomBEntrance));
        }

        public void RemoveConnectionsForRoom(Room room)
        {
            connections = connections.Where(
                roomConnection => !roomConnection.ContainsRoom(room)
            ).ToList();
        }

        public void RemoveConnectionsBetween(Room roomA, Room roomB)
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
            List<RoomConnection> result = new List<RoomConnection>();

            foreach (RoomEntrance targetRoomEntrance in targetRoom.entrances)
            {
                CellCoordinates targetCell;
                if (targetRoomEntrance.position == RoomEntrancePosition.Left)
                {
                    targetCell = targetRoomEntrance.cellCoordinates.Add(new CellCoordinates(-1, 0));
                }
                else
                {
                    // RoomEntrancePosition.Right
                    targetCell = targetRoomEntrance.cellCoordinates.Add(new CellCoordinates(1, 0));
                }

                Room roomAtCell = roomList.FindRoomAtCell(targetCell);

                if (roomAtCell != null)
                {
                    foreach (RoomEntrance roomAtCellEntrance in roomAtCell.entrances)
                    {
                        if (
                            roomAtCellEntrance.cellCoordinates.Matches(targetCell) &&
                            (
                                (
                                    targetRoomEntrance.position == RoomEntrancePosition.Left &&
                                    roomAtCellEntrance.position == RoomEntrancePosition.Right
                                ) || (
                                    targetRoomEntrance.position == RoomEntrancePosition.Right &&
                                    roomAtCellEntrance.position == RoomEntrancePosition.Left
                                )
                            )
                        )
                        {
                            RoomConnection newRoomConnection = new RoomConnection(targetRoom, targetRoomEntrance, roomAtCell, roomAtCellEntrance);
                            result.Add(newRoomConnection);
                        }
                    }
                }
            }

            return new RoomConnections(result);
        }
    }
}


