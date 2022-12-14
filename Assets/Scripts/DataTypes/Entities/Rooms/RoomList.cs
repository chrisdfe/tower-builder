using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Rooms
{
    public class RoomList : ListWrapper<Room>
    {
        public RoomList() : base() { }
        public RoomList(Room room) : base(room) { }
        public RoomList(List<Room> rooms) : base(rooms) { }
        public RoomList(RoomList roomList) : base(roomList) { }

        public Room FindRoomAtCell(CellCoordinates targetCellCoordinates) =>
            items.Find(room => room.cellCoordinatesList.Contains(targetCellCoordinates));
    }
}


