// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;

// namespace TowerBuilder.Stores.Map
// {
//     //
//     // Helpers for working with the dimensions of a room and roomShapes
//     //
//     public partial class MapStore
//     {
//         public static partial class Helpers
//         {
//             // TODO - rewrite for 3D
//             // public static bool roomIsOnFloor(int floor, List<MapCoordinates> roomCells)
//             // {
//             //     foreach (MapCoordinates roomCell in roomCells)
//             //     {
//             //         if (roomCell.y == floor)
//             //         {
//             //             return true;
//             //         }
//             //     }

//             //     return false;
//             // }

//             // public static List<int> getRoomFloors(List<MapCoordinates> roomCells)
//             // {
//             //     List<int> result = new List<int>();

//             //     roomCells.ForEach(roomCell =>
//             //     {
//             //         if (!result.Contains(roomCell.y))
//             //         {
//             //             result.Add(roomCell.y);
//             //         }
//             //     });

//             //     return result;
//             // }

//             // public static List<string> filterRoomIdsByFloor(
//             //     int floor,
//             //     RoomCellsMap roomCellsMap
//             // )
//             // {
//             //     return roomCellsMap.Keys.ToList().FindAll(roomId =>
//             //     {
//             //         List<MapCoordinates> roomShape = roomCellsMap[roomId];
//             //         List<int> roomFloors = getRoomFloors(roomShape);
//             //         return roomFloors.Contains(floor);
//             //     });
//             // }

//             // public static RoomCellsMap filterRoomsByFloor(
//             //     int floor,
//             //     RoomCellsMap roomCellsMap
//             // )
//             // {
//             //     List<string> roomIds = filterRoomIdsByFloor(floor, roomCellsMap);

//             //     RoomCellsMap result = new RoomCellsMap();

//             //     foreach (string roomId in roomCellsMap.Keys)
//             //     {
//             //         List<MapCoordinates> roomCells = roomCellsMap[roomId];
//             //         if (roomIsOnFloor(floor, roomCells))
//             //         {
//             //             result.Add(roomId, roomCells);
//             //         }
//             //     }

//             //     return result;
//             // }
//         }
//     }
// }
