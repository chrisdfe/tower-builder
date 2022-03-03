using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TowerBuilder.Stores.Map
{
    //
    // Helpers for working with the dimensions of a room and roomShapes
    //
    public partial class MapStore
    {
        public static partial class Helpers
        {
            // public static (int lowestX, int highestX) getLowestAndHighestX(List<MapCoordinates> roomShape)
            // {
            //     int lowestX = getLowestX(roomShape);
            //     int highestX = getHighestX(roomShape);
            //     return (lowestX, highestX);
            // }

            // public static (int lowestX, int highestX) getLowestAndHighestY(List<MapCoordinates> roomShape)
            // {
            //     int lowestY = getLowestY(roomShape);
            //     int highestY = getHighestY(roomShape);
            //     return (lowestY, highestY);
            // }

            // public static RoomShape createRectangularRoomShape(
            //   int width,
            //   int height
            // )
            // {
            //     List<MapCoordinates> result = new List<MapCoordinates>();

            //     for (int x = 0; x < width; x++)
            //     {
            //         for (int y = 0; y < height; y++)
            //         {
            //             result.Add(new MapCoordinates() { x = x, y = y });
            //         }
            //     }

            //     return result as RoomShape;
            // }

            public static List<CellCoordinates> create2DRectangularRoomBlueprint(int xSize, int floors)
            {
                List<CellCoordinates> result = new List<CellCoordinates>();

                for (int x = 0; x < xSize; x++)
                {
                    for (int floor = 0; floor < floors; floor++)
                    {
                        result.Add(new CellCoordinates()
                        {
                            x = x,
                            // z = z,
                            floor = floor
                        });
                    }
                }

                return result;
            }

            // public static List<MapCoordinates> createRectangularRoomShape(
            //   MapCoordinates startCoordinates,
            //   MapCoordinates endCoordinates
            // )
            // {
            //     List<MapCoordinates> bounds = new List<MapCoordinates>() { startCoordinates, endCoordinates };
            //     var (lowestX, highestX) = getLowestAndHighestX(bounds);
            //     var (lowestY, highestY) = getLowestAndHighestY(bounds);

            //     List<MapCoordinates> result = new List<MapCoordinates>();

            //     for (int x = lowestX; x <= highestX; x++)
            //     {
            //         for (int y = lowestY; y <= highestY; y++)
            //         {
            //             result.Add(new MapCoordinates() { x = x, y = y });
            //         }
            //     }

            //     return result;
            // }
        }
    }
}
