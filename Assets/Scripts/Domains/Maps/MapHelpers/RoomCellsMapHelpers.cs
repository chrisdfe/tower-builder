using System;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    //
    // Helpers for working with RoomCellsMaps
    //
    public static class RoomCellsMapHelpers
    {
        public static List<MapCoordinates> findRoomCellsById(string roomId, RoomCellsMap roomCellsMap)
        {
            return roomCellsMap[roomId];
        }

        public static string findRoomIdAtCell(
            MapCoordinates coordinates,
            RoomCellsMap roomCellsMap
        )
        {
            foreach (string roomId in roomCellsMap.Keys)
            {
                List<MapCoordinates> roomCells = roomCellsMap[roomId];
                if (MapCollisionHelpers.cellIntersectsCoordinatesList(coordinates, roomCells))
                {
                    return roomId;
                }
            }
            return null;
        }
    }
}