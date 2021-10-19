using System;
using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    public partial class RoomCells
    {
        public static int getLowestX(List<CellCoordinates> roomCells)
        {
            int lowestX = int.MaxValue;

            foreach (CellCoordinates cellCoordinates in roomCells)
            {
                if (cellCoordinates.x < lowestX)
                {
                    lowestX = cellCoordinates.x;
                }
            }

            return lowestX;
        }

        public int getLowestX()
        {
            return getLowestX(cells);
        }

        public static int getHighestX(List<CellCoordinates> roomCells)
        {
            int highestX = int.MinValue;

            foreach (CellCoordinates cellCoordinates in roomCells)
            {
                if (cellCoordinates.x > highestX)
                {
                    highestX = cellCoordinates.x;
                }
            }

            return highestX;
        }

        public int getHighestX()
        {
            return getHighestX(cells);
        }

        // TODO - same for Z
        // TODO - same for floor
    }
}


