using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Connections
{
    public class RoomConnections
    {
        public List<RoomConnection> connections = new List<RoomConnection>();

        public RoomConnections() { }
        public RoomConnections(List<RoomConnection> connections)
        {
            this.connections = connections;
        }

        public void Add(RoomConnections roomConnections)
        {
            connections = connections.Concat(roomConnections.connections).ToList();
        }

        public void Remove(RoomConnections roomConnections)
        {
            connections.RemoveAll(roomConnection => roomConnections.connections.Contains(roomConnection));
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


