// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;

// namespace TowerBuilder.Stores.Map
// {
//     //
//     // Helpers for adding/removing/combining rooms
//     //
//     public partial class MapStore
//     {
//         public static partial class Helpers
//         {
//             // TODO - rewrite for c#
//             // public static List<MapCoordinates> addCellToRoom(
//             //   MapCoordinates coordinates,
//             //   List<MapCoordinates> roomCells
//             // )
//             // {
//             //     // Make sure we don't add the same cell twice
//             //     if (MapStore.Helpers.cellIntersectsCoordinatesList(coordinates, roomCells))
//             //     {
//             //         return roomCells;
//             //     }

//             //     roomCells.Add(coordinates);
//             //     return roomCells;
//             // }

//             // public static List<MapCoordinates> addCellsToRoom(
//             //   List<MapCoordinates> currentRoomCells,
//             //   List<MapCoordinates> newRoomCells
//             // )
//             // {
//             //     return currentRoomCells.Concat(newRoomCells).ToList();
//             // }

//             // public static List<MapCoordinates> removeCellFromRoom(
//             //   List<MapCoordinates> roomCells,
//             //   MapCoordinates targetCoordinates
//             // )
//             // {
//             //     return roomCells.FindAll(roomCell =>
//             //     {
//             //         return !MapStore.Helpers.coordinatesMatch(targetCoordinates, roomCell);
//             //     });
//             // }

//             // public static List<MapCoordinates> removeCellsFromRoom(
//             //   List<MapCoordinates> roomCells,
//             //   List<MapCoordinates> roomCellsToRemove
//             // )
//             // {
//             //     return roomCells.FindAll(roomCell =>
//             //     {
//             //         return !MapStore.Helpers.cellIntersectsCoordinatesList(roomCell, roomCellsToRemove);
//             //     });
//             // }
//         }
//     }
// }