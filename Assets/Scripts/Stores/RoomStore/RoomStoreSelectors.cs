using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerBuilder.Stores
{
    public static class RoomStoreSelectors
    {
        public static RoomKey findRoomKeyById(StoreRegistry storeRegistry, string roomId)
        {
            return storeRegistry.roomStore.state.roomKeyMap[roomId];
        }

        public static RoomDetails findRoomDetailsById(StoreRegistry storeRegistry, string roomId)
        {
            RoomKey roomKey = findRoomKeyById(storeRegistry, roomId);
            RoomDetails roomDetails = RoomStoreConstants.ROOM_DETAILS_MAP[roomKey];
            return roomDetails;
        }

        public static List<string> getRoomIds(StoreRegistry storeRegistry)
        {
            RoomStore roomStore = storeRegistry.roomStore;

            return roomStore.state.roomKeyMap.Keys.ToList();
        }

        public static List<string> filterRoomIdsByRoomKey(StoreRegistry storeRegistry, RoomKey targetRoomKey)
        {
            RoomStore roomStore = storeRegistry.roomStore;
            RoomKeyMap roomKeyMap = roomStore.state.roomKeyMap;
            List<string> roomIds = getRoomIds(storeRegistry);

            return roomIds.FindAll(id => roomKeyMap[id] == targetRoomKey);
        }
    }
}