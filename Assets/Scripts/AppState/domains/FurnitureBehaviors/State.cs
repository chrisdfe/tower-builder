using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.DataTypes.Rooms;
using TowerBuilder.DataTypes.Time;

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
            Registry.appState.Rooms.events.onRoomAdded += OnRoomAdded;
        }

        void OnTick(TimeValue timeValue)
        {

        }

        void OnRoomAdded(Room room)
        {

        }
    }
}
