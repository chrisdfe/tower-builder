using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores;

namespace TowerBuilder.Stores.Map
{
    public class State
    {
        List<MapRoom> mapRooms;

        public delegate void RoomAddedEvent(MapRoom mapRoom);
        public RoomAddedEvent onRoomAdded;

        public State()
        {
            mapRooms = new List<MapRoom>();
        }

        // TODO - maybe all mutations should be like this instead
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
