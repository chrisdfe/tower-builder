using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerBuilder.Domains.Rooms
{
    public static class RoomSelectors
    {
        public static RoomKey findRoomKeyById(StoreRegistry storeRegistry, string roomId)
        {
            return RoomHelpers.findRoomKeyById(roomId, storeRegistry.roomStore.state.roomKeyMap);
        }

        public static RoomDetails findRoomDetailsById(StoreRegistry storeRegistry, string roomId)
        {
            return RoomHelpers.findRoomDetailsById(roomId, storeRegistry.roomStore.state.roomKeyMap);
        }

        public static List<string> getRoomIds(StoreRegistry storeRegistry)
        {
            return RoomHelpers.getRoomIds(storeRegistry.roomStore.state.roomKeyMap);
        }

        public static List<string> filterRoomIdsByRoomKey(StoreRegistry storeRegistry, RoomKey roomKey)
        {
            return RoomHelpers.filterRoomIdsByRoomKey(storeRegistry.roomStore.state.roomKeyMap, roomKey);
        }
    }
}