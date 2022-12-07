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

        public Room FindRoomAtCell(CellCoordinates targetCellCoordinates)
        {
            foreach (Room room in items)
            {
                foreach (RoomCell roomCell in room.blocks.cells.cells)
                {
                    if (roomCell.coordinates.Matches(targetCellCoordinates))
                    {
                        return room;
                    }
                }
            }

            return null;
        }

        public Room FindRoomByRoomBlock(RoomCells roomBlock)
        {
            foreach (Room room in items)
            {
                if (room.blocks.ContainsBlock(roomBlock))
                {
                    return room;
                }
            }

            return null;
        }
    }
}


