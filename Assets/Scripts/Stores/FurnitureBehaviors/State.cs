using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Furniture;
using TowerBuilder.Stores.Time;

namespace TowerBuilder.Stores.FurnitureBehaviors
{
    public class State
    {
        public State()
        {
            Registry.Stores.Time.onTick += OnTick;
            Registry.Stores.Rooms.onRoomAdded += OnRoomAdded;
        }

        void OnTick(TimeValue timeValue)
        {

        }

        void OnRoomAdded(Room room)
        {

        }
    }
}
