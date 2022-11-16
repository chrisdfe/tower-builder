using TowerBuilder.DataTypes.Time;
using TowerBuilder.State;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class IdleStateHandler : StateHandlerBase
    {
        public class TransitionPayload : TransitionPayloadBase
        {
            public override ResidentBehavior.StateType stateType { get { return ResidentBehavior.StateType.Idle; } }
        }

        public IdleStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior)
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
            Debug.Log("Idle");
        }
    }
}