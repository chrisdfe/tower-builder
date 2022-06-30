using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.State;
using TowerBuilder.State.Rooms;
using TowerBuilder.State.Rooms.Furniture;
using TowerBuilder.State.Time;

namespace TowerBuilder.State.FurnitureBehaviors
{
    public class State
    {
        public State()
        {
        }

        public void Setup()
        {
            Registry.appState.Time.onTick += OnTick;
            Registry.appState.Rooms.onRoomAdded += OnRoomAdded;
        }

        void OnTick(TimeValue timeValue)
        {

        }

        void OnRoomAdded(Room room)
        {

        }
    }
}
