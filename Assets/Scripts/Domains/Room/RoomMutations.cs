using System;

namespace TowerBuilder.Domains.Rooms
{
    public static class RoomMutations
    {
        public static string addRoomKeyToMap(StoreRegistry storeRegistry, RoomKey roomKey)
        {
            string roomId = Guid.NewGuid().ToString();
            storeRegistry.roomStore.state.roomKeyMap.Add(roomId, roomKey);
            return roomId;
        }

        public static void buildRoom() { }
    }
}