using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Rooms
{
    public static class RoomHelpers
    {
        public static RoomKey findRoomKeyById(string roomId, RoomKeyMap roomKeyMap)
        {
            return roomKeyMap[roomId];
        }

        public static RoomDetails findRoomDetailsById(string roomId, RoomKeyMap roomKeyMap)
        {
            RoomKey roomKey = findRoomKeyById(roomId, roomKeyMap);
            RoomDetails roomDetails = RoomConstants.ROOM_DETAILS_MAP[roomKey];
            return roomDetails;
        }

        public static List<string> getRoomIds(RoomKeyMap roomKeyMap)
        {
            return roomKeyMap.Keys.ToList();
        }

        public static List<string> filterRoomIdsByRoomKey(RoomKeyMap roomKeyMap, RoomKey roomKey)
        {
            List<string> roomIds = getRoomIds(roomKeyMap);
            return roomIds.FindAll(id => roomKeyMap[id] == roomKey);
        }
    }
}