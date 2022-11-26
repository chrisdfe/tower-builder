using System.Collections.Generic;
using System.Linq;
using TowerBuilder.DataTypes.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Connections
{
    public class RoomConnectionList : ListWrapper<RoomConnection>
    {
        public RoomConnectionList() : base() { }
        public RoomConnectionList(RoomConnection item) : base(item) { }
        public RoomConnectionList(List<RoomConnection> items) : base(items) { }
        public RoomConnectionList(RoomConnectionList roomConnectionList) : base(roomConnectionList) { }

        public RoomConnectionList FindConnectionsForRoom(Room room)
        {
            return new RoomConnectionList(
                items.FindAll(roomConnection => roomConnection.ContainsRoom(room))
            );
        }

        public RoomConnection FindConnectionForRoomEntrance(RoomEntrance roomEntrance)
        {
            return items.Find(roomConnection => roomConnection.ContainsRoomEntrance(roomEntrance));
        }

        public RoomConnectionList SearchForNewConnectionsToRoom(RoomList roomList, Room targetRoom)
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

            return new RoomConnectionList(result);
        }
    }
}


