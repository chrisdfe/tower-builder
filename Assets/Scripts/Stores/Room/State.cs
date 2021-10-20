using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Rooms
{
    public partial class State
    {
        public List<string> roomIds { get; private set; }
        public Dictionary<string, RoomKey> roomKeyMap { get; private set; }

        public delegate void RoomCreatedEvent(string roomId);
        public static event RoomCreatedEvent onRoomCreated;

        public delegate void RoomDestroyedEvent(string roomId);
        public static event RoomDestroyedEvent onRoomDestroyed;

        public State()
        {
            roomIds = new List<string>();
            roomKeyMap = new Dictionary<string, RoomKey>();
        }

        public RoomKey FindRoomKeyById(string roomId)
        {
            return roomKeyMap[roomId];
        }

        public RoomDetails FindRoomDetailsById(string roomId)
        {
            RoomKey roomKey = FindRoomKeyById(roomId);
            RoomDetails roomDetails = Constants.ROOM_DETAILS_MAP[roomKey];
            return roomDetails;
        }
    }
}
