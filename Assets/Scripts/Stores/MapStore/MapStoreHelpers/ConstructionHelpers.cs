using System;
using System.Collections;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    //
    // Helpers for adding/removing/combining rooms
    //
    public static class MapStoreConstructionHelpers
    {
        public static List<MapCoordinates> addCellToRoom(
          MapCoordinates coordinates,
          List<MapCoordinates> roomCells
        )
        {
            // Make sure we don't add the same cell twice
            if (MapStoreCollisionHelpers.cellIntersectsCoordinatesList(coordinates, roomCells))
            {
                return roomCells;
            }

            roomCells.Add(coordinates);
            return roomCells;
            // return [...roomCells, coordinates];
        }

        // export const addCellsToRoom = (
        //   currentRoomCells: MapCoordinates[],
        //   newRoomCells: MapCoordinates[]
        // ): MapCoordinates[] => {
        //     return [...currentRoomCells, ...newRoomCells];
        // };

        // export const removeCellFromRoom = (
        //   roomCells: MapCoordinates[],
        //   targetCoordinates: MapCoordinates
        // ): MapCoordinates[] => {
        //     return roomCells.filter((roomCell) =>
        //     {
        //         return !coordinatesMatch(targetCoordinates, roomCell);
        //     });
        // };

        // export const removeCellsFromRoom = (
        //   roomCells: MapCoordinates[],
        //   roomCellsToRemove: MapCoordinates[]
        // ) => {
        //   return roomCells.filter((roomCell) => {
        //     return !cellIntersectsCoordinatesList(roomCell, roomCellsToRemove);
        // });
        // };
    }
}