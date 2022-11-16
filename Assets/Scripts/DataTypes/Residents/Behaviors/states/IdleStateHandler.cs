using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class IdleStateHandler : StateHandlerBase
    {
        public class TransitionPayload : TransitionPayloadBase
        {
        }

        public IdleStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior)
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
            Debug.Log("Idle");
        }
    }
}