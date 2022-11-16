using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class InteractingWithFurnitureStateHandler : StateHandlerBase
    {
        public class TransitionPayload : TransitionPayloadBase
        {
            public override ResidentBehavior.StateType stateType { get { return ResidentBehavior.StateType.InteractingWithFurniture; } }
            public Furniture furniture;
        }

        public Furniture furniture;

        public InteractingWithFurnitureStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior)
        {
        }

        public override void Setup()
        {
            base.Setup();
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override TransitionPayloadBase GetNextState(AppState appState)
        {
            return null;
        }

        public override void ProcessTick(AppState appState)
        {
            Debug.Log("Traveling");
        }
    }
}