using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Connections
{
    public class RoomConnections
    {
        public List<RoomConnection> connections = new List<RoomConnection>();

        public int Count { get { return connections.Count; } }

        public RoomConnections() { }
        public RoomConnections(List<RoomConnection> connections)
        {
            this.connections = connections;
        }

        public void Add(RoomConnections roomConnections)
        {
            connections = connections.Concat(roomConnections.connections).ToList();
        }

        public void Remove(RoomConnection roomConnection)
        {
            connections.Remove(roomConnection);
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

        public RoomConnections SearchForNewConnectionsToRoom(RoomList roomList, Room targetRoom)
        {
            List<RoomConnection> result = new List<RoomConnection>();

            foreach (RoomEntrance targetRoomEntrance in targetRoom.entrances)
            {
                CellCoordinates targetCell;
                if (targetRoomEntrance.position == RoomEntrance.Position.Left)
                {
                    targetCell = targetRoomEntrance.cellCoordinates.Add(new CellCoordinates(-1, 0));
                }
                else
                {
                    // RoomEntrance.Position.Right
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
                                    targetRoomEntrance.position == RoomEntrance.Position.Left &&
                                    roomAtCellEntrance.position == RoomEntrance.Position.Right
                                ) || (
                                    targetRoomEntrance.position == RoomEntrance.Position.Right &&
                                    roomAtCellEntrance.position == RoomEntrance.Position.Left
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


