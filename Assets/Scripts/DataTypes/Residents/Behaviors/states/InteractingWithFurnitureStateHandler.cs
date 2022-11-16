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
            public Furniture furniture;
        }

        public Furniture furniture;

        public InteractingWithFurnitureStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior)
        {
        }

        public void Setup(TransitionPayload payload)
        {
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