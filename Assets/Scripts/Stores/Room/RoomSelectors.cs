using System;
using System.Linq;
using System.Collections.Generic;

namespace TowerBuilder.Stores.Rooms
{
    public partial class RoomStore
    {
        public static class Selectors
        {
            public static RoomKey findRoomKeyById(StoreRegistry storeRegistry, string roomId)
            {
                return RoomStore.Helpers.findRoomKeyById(roomId, storeRegistry.roomStore.state.roomKeyMap);
            }

            public static RoomDetails findRoomDetailsById(StoreRegistry storeRegistry, string roomId)
            {
                return RoomStore.Helpers.findRoomDetailsById(roomId, storeRegistry.roomStore.state.roomKeyMap);
            }

            public static List<string> getRoomIds(StoreRegistry storeRegistry)
            {
                return RoomStore.Helpers.getRoomIds(storeRegistry.roomStore.state.roomKeyMap);
            }

            public static List<string> filterRoomIdsByRoomKey(StoreRegistry storeRegistry, RoomKey roomKey)
            {
                return RoomStore.Helpers.filterRoomIdsByRoomKey(storeRegistry.roomStore.state.roomKeyMap, roomKey);
            }
        }
    }
}