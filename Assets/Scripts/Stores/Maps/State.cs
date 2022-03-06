using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Map
{
    public class State
    {
        public List<Room> mapRooms { get; private set; }

        public delegate void RoomAddedEvent(Room mapRoom);
        public RoomAddedEvent onRoomAdded;

        public State()
        {
            mapRooms = new List<Room>();
        }


        public void AddRoom(Room room)
        {
            mapRooms.Add(room);

            if (onRoomAdded != null)
            {
                onRoomAdded(room);
            }
        }
    }
}