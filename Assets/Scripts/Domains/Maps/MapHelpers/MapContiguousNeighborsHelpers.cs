using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    //
    // Helpers finding and querying contiguous floor neighbors
    //
    public static class MapContiguousNeighborsHelpers
    {

        // TODO - I could probably make this more efficient by using
        //        the room's roomCells instead of iterating through every cell
        //        of a room and seeing if there is a room there.
        public static List<MapCoordinates> getContiguousRoomCellsOnFloor(
          List<MapCoordinates> roomCells,
          RoomCellsMap roomCellsMap
        )
        {
            List<MapCoordinates> cellsToTheLeft = new List<MapCoordinates>();
            List<MapCoordinates> cellsToTheRight = new List<MapCoordinates>();
            List<MapCoordinates> result = new List<MapCoordinates>();

            // TODO - use all room floors
            int roomFloor = MapFloorHelpers.getRoomFloors(roomCells)[0];
            var (lowestX, highestX) = MapDimensionsHelpers.getLowestAndHighestX(roomCells);

            // go from cell x all the way to 0
            for (int currentX = lowestX - 1; currentX >= 0; currentX--)
            {
                MapCoordinates cellCoordinates = new MapCoordinates()
                {
                    x = currentX,
                    y = roomFloor
                };

                // Check if there is a room there
                string roomId = RoomCellsMapHelpers.findRoomIdAtCell(cellCoordinates, roomCellsMap);
                // If not then it we have already reached the edge of the contiguous floor
                if (roomId == null)
                {
                    break;
                }

                cellsToTheLeft.Add(cellCoordinates);
            }

            // go from cell x all the way up to the max x
            // TODO - use constant for the max amount
            for (int currentX = highestX + 1; currentX <= 50; currentX++)
            {
                MapCoordinates cellCoordinates = new MapCoordinates()
                {
                    x = currentX,
                    y = roomFloor
                };

                // Check if there is a room there - if not then it is no longer contiguous
                string roomId = RoomCellsMapHelpers.findRoomIdAtCell(cellCoordinates, roomCellsMap);
                // If not then it we have already reached the edge of the contiguous floor
                if (roomId == null)
                {
                    break;
                }

                cellsToTheRight.Add(cellCoordinates);
            }

            result = cellsToTheLeft.Concat(cellsToTheRight).ToList();
            return result;
        }

        public static List<string> findContiguousRoomIdsOnFloor(
          List<MapCoordinates> roomCells,
          RoomCellsMap roomCellsMap
        )
        {
            List<MapCoordinates> contigiousRoomCells = getContiguousRoomCellsOnFloor(
              roomCells,
              roomCellsMap
            );

            List<string> result = new List<string>();

            foreach (MapCoordinates coordinates in contigiousRoomCells)
            {
                string roomId = RoomCellsMapHelpers.findRoomIdAtCell(coordinates, roomCellsMap);
                if (!result.Contains(roomId))
                {
                    result.Add(roomId);
                }
            }

            return result;
        }
    }
}