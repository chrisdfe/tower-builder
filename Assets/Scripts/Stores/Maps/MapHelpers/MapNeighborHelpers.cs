
using System;
using System.Collections.Generic;

using TowerBuilder.Stores.Rooms;

namespace TowerBuilder.Stores.Map
{
    //
    // Helpers for working with direct room neighbors
    //
    public partial class MapStore
    {
        public static partial class Helpers
        {
            public static MapCoordinates getCoordinatesBelow(MapCoordinates coordinates)
            {
                return new MapCoordinates()
                {
                    x = coordinates.x,
                    y = coordinates.y - 1
                };
            }

            public static MapCoordinates getCoordinatesAbove(MapCoordinates coordinates)
            {
                return new MapCoordinates()
                {
                    x = coordinates.x,
                    y = coordinates.y + 1
                };
            }

            public static MapCoordinates getCoordinatesToTheLeft(MapCoordinates coordinates)
            {
                return new MapCoordinates()
                {
                    x = coordinates.x - 1,
                    y = coordinates.y
                };
            }

            public static MapCoordinates getCoordinatesToTheRight(MapCoordinates coordinates)
            {
                return new MapCoordinates()
                {
                    x = coordinates.x - 1,
                    y = coordinates.y
                };
            }

            public static string getRoomIdBelow(
              MapCoordinates coordinates,
              RoomCellsMap roomCellsMap
            )
            {
                MapCoordinates cellBelow = getCoordinatesBelow(coordinates);
                return MapStore.Helpers.findRoomIdAtCell(cellBelow, roomCellsMap);
            }

            public static string getRoomIdAbove(
              MapCoordinates coordinates,
              RoomCellsMap roomCellsMap
            )
            {
                MapCoordinates cellBelow = getCoordinatesAbove(coordinates);
                return MapStore.Helpers.findRoomIdAtCell(cellBelow, roomCellsMap);
            }

            public static string getRoomIdToTheLeft(
              MapCoordinates coordinates,
              RoomCellsMap roomCellsMap
            )
            {
                MapCoordinates cellBelow = getCoordinatesToTheLeft(coordinates);
                return MapStore.Helpers.findRoomIdAtCell(cellBelow, roomCellsMap);
            }

            public static string getRoomIdToTheRight(
              MapCoordinates coordinates,
              RoomCellsMap roomCellsMap
            )
            {
                MapCoordinates cellBelow = getCoordinatesToTheRight(coordinates);
                return MapStore.Helpers.findRoomIdAtCell(cellBelow, roomCellsMap);
            }

            public static (string, string) getHorizontalNeighbors(
                MapCoordinates coordinates,
                RoomCellsMap roomCellsMap
            )
            {
                return (
                    getRoomIdToTheLeft(coordinates, roomCellsMap),
                    getRoomIdToTheRight(coordinates, roomCellsMap)
                );
            }

            // TODO -
            // public static (string, string) getHorizontalNeighbors(string roomId, RoomCellsMap roomCellsMap)

            public static (string, string) getVerticalNeighbors(
                MapCoordinates coordinates,
                RoomCellsMap roomCellsMap
            )
            {
                return (
                    getRoomIdAbove(coordinates, roomCellsMap),
                    getRoomIdBelow(coordinates, roomCellsMap)
                );
            }

            public static (string, string, string, string) getOrthogonalNeighbors(
              MapCoordinates coordinates,
              RoomCellsMap roomCellsMap
            )
            {
                return (
                  getRoomIdAbove(coordinates, roomCellsMap),
                  getRoomIdToTheRight(coordinates, roomCellsMap),
                  getRoomIdBelow(coordinates, roomCellsMap),
                  getRoomIdToTheLeft(coordinates, roomCellsMap)
                );
            }
        }
    }
}