using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Map
{
    public class State
    {
        public List<MapRoom> mapRooms { get; private set; }

        public delegate void RoomAddedEvent(MapRoom mapRoom);
        public RoomAddedEvent onRoomAdded;

        public State()
        {
            mapRooms = new List<MapRoom>();
        }


        public void AddRoom(MapRoom newRoom)
        {
            mapRooms.Add(newRoom);

            if (onRoomAdded != null)
            {
                onRoomAdded(newRoom);
            }
        }
    }
}
