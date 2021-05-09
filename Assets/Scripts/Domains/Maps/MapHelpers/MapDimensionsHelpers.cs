using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    //
    // Helpers for working with the dimensions of a room and roomShapes
    //
    public static class MapDimensionsHelpers
    {
        public static int getLowestY(List<MapCoordinates> roomShape)
        {
            int lowestY = int.MaxValue;

            foreach (MapCoordinates coordinates in roomShape)
            {
                if (coordinates.y < lowestY)
                {
                    return coordinates.y;
                }
            }

            return lowestY;
        }

        public static int getHighestY(List<MapCoordinates> roomShape)
        {
            int highestY = int.MinValue;

            foreach (MapCoordinates coordinates in roomShape)
            {
                if (coordinates.y > highestY)
                {
                    return coordinates.y;
                }
            }

            return highestY;
        }

        public static int getLowestX(List<MapCoordinates> roomShape)
        {
            int lowestX = int.MaxValue;

            foreach (MapCoordinates coordinates in roomShape)
            {
                if (coordinates.x < lowestX)
                {
                    return coordinates.x;
                }
            }

            return lowestX;
        }

        public static int getHighestX(List<MapCoordinates> roomShape)
        {
            int highestX = int.MinValue;

            foreach (MapCoordinates coordinates in roomShape)
            {
                if (coordinates.x > highestX)
                {
                    return coordinates.x;
                }
            }

            return highestX;
        }

        public static (int lowestX, int highestX) getLowestAndHighestX(List<MapCoordinates> roomShape)
        {
            int lowestX = getLowestX(roomShape);
            int highestX = getHighestX(roomShape);
            return (lowestX, highestX);
        }

        public static (int lowestX, int highestX) getLowestAndHighestY(List<MapCoordinates> roomShape)
        {
            int lowestY = getLowestY(roomShape);
            int highestY = getHighestY(roomShape);
            return (lowestY, highestY);
        }

        public static List<MapCoordinates> createRectangularRoomShape(
          int width,
          int height
        )
        {
            List<MapCoordinates> result = new List<MapCoordinates>();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    result.Add(new MapCoordinates() { x = x, y = y });
                }
            }

            return result;
        }

        public static List<MapCoordinates> createRectangularRoomShape(
          MapCoordinates startCoordinates,
          MapCoordinates endCoordinates
        )
        {
            List<MapCoordinates> bounds = new List<MapCoordinates>() { startCoordinates, endCoordinates };
            var (lowestX, highestX) = getLowestAndHighestX(bounds);
            var (lowestY, highestY) = getLowestAndHighestY(bounds);

            List<MapCoordinates> result = new List<MapCoordinates>();

            for (int x = lowestX; x <= highestX; x++)
            {
                for (int y = lowestY; y <= highestY; y++)
                {
                    result.Add(new MapCoordinates() { x = x, y = y });
                }
            }

            return result;
        }

        // TODO - roomgrouphelpers
        // public static List<MapCoordinates> getRoomOrRoomGroupCells(
        //   string roomId,
        //   RoomCellsMap roomCellsMap,
        //   RoomGroupMap roomGroupMap
        // )
        // {
        //     string roomGroupId = roomGroupMap[roomId];

        //     if (roomGroupId == null)
        //     {
        //         return roomCellsMap[roomId];
        //     }

        //     const roomIds = mapStore.state.roomGroupMap[roomGroupId];
        //     const roomGroupCells: MapCoordinates[] = flatMap(roomIds, (roomId) =>
        //     {
        //     const roomCells: MapCoordinates[] = roomCellsMap[roomId];
        //     return roomCells;
        // });

        //   return roomGroupCells;
        // }

        // public static void getRoomOrRoomGroupSize = (
        //   roomId: RoomId,
        //   roomCellsMap: RoomCellsMap
        // ) => {
        //   const cells = getRoomOrRoomGroupCells(roomId, roomCellsMap);
        //   return cells.length;
        // };
    }
}
