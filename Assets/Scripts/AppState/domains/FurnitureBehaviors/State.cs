using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Furnitures.Behaviors;
using UnityEngine;

namespace TowerBuilder.ApplicationState.FurnitureBehaviors
{
    public class State : StateSlice
    {
        public class Input { }

        public class Events
        {

        }

        public FurnitureBehaviorList furnitureBehaviorList { get; private set; } = new FurnitureBehaviorList();

        public Events events { get; private set; }

        public State(AppState appState, Input input) : base(appState)
        {

        }
    }
}