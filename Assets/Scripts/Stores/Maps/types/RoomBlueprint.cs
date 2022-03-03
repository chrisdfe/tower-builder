using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class RoomBlueprint
    {
        public RoomKey roomKey;
        public CellCoordinates coordinates;

        public RoomCells GetRoomShape()
        {
            RoomCells roomCells;

            // TODO - not this
            if (roomKey == RoomKey.Condo)
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 3);
            }
            else if (roomKey == RoomKey.Office)
            {
                roomCells = RoomCells.CreateRectangularRoom(3, 2);
            }
            else
            {
                roomCells = RoomCells.CreateRectangularRoom(1, 1);
            }

            return roomCells;
        }

        public RoomCells GetPositionedRoomCells()
        {
            RoomCells roomShape = GetRoomShape();
            return RoomCells.PositionAtCoordinates(roomShape, coordinates);
        }
    }
}


