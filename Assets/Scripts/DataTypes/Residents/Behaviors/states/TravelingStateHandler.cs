using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class TravelingStateHandler : StateHandlerBase
    {
        public class TransitionPayload : TransitionPayloadBase
        {
            // For now assume that residents always travel between pieces of furniture
            public Furniture furniture;
        }

        public enum StateType
        {
            Walking,
            UsingStairs
        }

        public class Objective
        {
            public Furniture furniture { get; private set; }

            public Objective(Furniture furniture)
            {
                this.furniture = furniture;
            }
        }

        public StateType currentState { get; private set; } = StateType.Walking;
        public Objective curentObjective { get; private set; }

        public TravelingStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior)
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