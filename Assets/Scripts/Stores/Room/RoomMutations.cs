using System;

namespace TowerBuilder.Stores.Rooms
{
    public partial class RoomStore
    {
        public static class Mutations
        {
            public static string addRoomKeyToMap(RoomKey roomKey)
            {
                string roomId = Guid.NewGuid().ToString();
                Registry.Stores.roomStore.state.roomKeyMap.Add(roomId, roomKey);
                return roomId;
            }

            public static void buildRoom() { }
        }
    }
}