using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    //
    // Helpers for adding/removing/combining rooms
    //
    public static class MapConstructionHelpers
    {
        public static List<MapCoordinates> addCellToRoom(
          MapCoordinates coordinates,
          List<MapCoordinates> roomCells
        )
        {
            // Make sure we don't add the same cell twice
            if (MapCollisionHelpers.cellIntersectsCoordinatesList(coordinates, roomCells))
            {
                return roomCells;
            }

            roomCells.Add(coordinates);
            return roomCells;
        }

        public static List<MapCoordinates> addCellsToRoom(
          List<MapCoordinates> currentRoomCells,
          List<MapCoordinates> newRoomCells
        )
        {
            return currentRoomCells.Concat(newRoomCells).ToList();
        }

        public static List<MapCoordinates> removeCellFromRoom(
          List<MapCoordinates> roomCells,
          MapCoordinates targetCoordinates
        )
        {
            return roomCells.FindAll(roomCell =>
            {
                return !MapCollisionHelpers.coordinatesMatch(targetCoordinates, roomCell);
            });
        }

        public static List<MapCoordinates> removeCellsFromRoom(
          List<MapCoordinates> roomCells,
          List<MapCoordinates> roomCellsToRemove
        )
        {
            return roomCells.FindAll(roomCell =>
            {
                return !MapCollisionHelpers.cellIntersectsCoordinatesList(roomCell, roomCellsToRemove);
            });
        }
    }
}