using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Rooms
{
    public class RoomList : ResourceList<Room>
    {
        public delegate void RoomBlockEvent(RoomCells roomBlock);
        public RoomBlockEvent onRoomBlockRemoved;

        public override void Add(Room room)
        {
            base.Add(room);

            room.OnBuild();
            room.blocks.onItemRemoved += OnRoomBlockRemoved;
        }

        public override void Remove(Room room)
        {
            base.Remove(room);
            room.OnDestroy();
            room.blocks.onItemRemoved -= OnRoomBlockRemoved;
        }

        public void OnRoomBlockRemoved(RoomCells roomBlock)
        {
            if (onRoomBlockRemoved != null)
            {
                onRoomBlockRemoved(roomBlock);
            }
        }

        public Room FindRoomAtCell(CellCoordinates targetCellCoordinates)
        {
            foreach (Room room in items)
            {
                foreach (RoomCell roomCell in room.cells.items)
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
                if (room.ContainsBlock(roomBlock))
                {
                    return room;
                }
            }

            return null;
        }
    }
}


