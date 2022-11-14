using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.State.FurnitureBehaviors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {

        }

        public Events events { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {

        }
    }
}