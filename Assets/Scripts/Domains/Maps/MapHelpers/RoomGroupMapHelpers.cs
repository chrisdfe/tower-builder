using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Map
{
    //
    // Helpers for working with RoomCellsMaps
    //
    public static class RoomGroupMapHelpers
    {
        public static List<string> findRoomGroupById(
            string roomGroupId,
            RoomGroupMap roomGroupMap
        )
        {
            return roomGroupMap[roomGroupId];
        }

        public static string findRoomGroupIdByRoomId(
          string roomId,
          RoomGroupMap roomGroupMap
        )
        {
            return roomGroupMap.Keys.ToList().Find(roomGroupId =>
            {
                List<string> roomGroup = roomGroupMap[roomId];
                return roomGroup.Contains(roomGroupId);
            });
        }

        public static string findRoomGroupIdAtCell(
            MapCoordinates targetCoordinates,
            RoomCellsMap roomCellsMap,
            RoomGroupMap roomGroupMap
        )
        {
            string roomId = RoomCellsMapHelpers.findRoomIdAtCell(targetCoordinates, roomCellsMap);
            return findRoomGroupIdByRoomId(roomId, roomGroupMap);
        }
    }
}