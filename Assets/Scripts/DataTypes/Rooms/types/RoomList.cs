using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomList
    {
        public List<Room> rooms { get; private set; } = new List<Room>();

        public int Count { get { return rooms.Count; } }

        public RoomList() { }
        public RoomList(List<Room> rooms)
        {
            this.rooms = rooms;
        }

        public void Add(Room room)
        {
            if (!rooms.Contains(room))
            {
                rooms.Add(room);
            }
        }

        public void Remove(Room room)
        {
            rooms.Remove(room);
        }

        public Room FindRoomAtCell(CellCoordinates targetCellCoordinates)
        {
            foreach (Room room in rooms)
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
            foreach (Room room in rooms)
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


