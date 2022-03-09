using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;
using TowerBuilder.Stores.Map.Rooms;

namespace TowerBuilder.Stores.Map
{
    public class State
    {
        public RoomList rooms { get; private set; }

        public delegate void RoomAddedEvent(Room mapRoom);
        public RoomAddedEvent onRoomAdded;

        public delegate void RoomDestroyedEvent(Room mapRoom);
        public RoomAddedEvent onRoomDestroyed;

        public State()
        {
            rooms = new RoomList();
        }


        public void AddRoom(Room room)
        {
            rooms.Add(room);

            if (onRoomAdded != null)
            {
                onRoomAdded(room);
            }
        }

        public void DestroyRoom(Room room)
        {
            rooms.Remove(room);

            if (onRoomDestroyed != null)
            {
                onRoomDestroyed(room);
            }
        }
    }
}
