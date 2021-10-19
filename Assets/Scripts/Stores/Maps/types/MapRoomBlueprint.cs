using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class MapRoomBlueprint
    {
        public RoomKey roomKey;
        public CellCoordinates coordinates;
        public MapRoomRotation rotation;

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

        public RoomCells GetRotatedRoomCells()
        {
            RoomCells roomCells = GetRoomShape();
            return RoomCells.Rotate(roomCells, rotation);
        }

        public RoomCells GetPositionedRoomCells()
        {
            RoomCells rotatedRoomCells = GetRotatedRoomCells();
            return RoomCells.PositionAtCoordinates(rotatedRoomCells, coordinates);
        }
    }
}


