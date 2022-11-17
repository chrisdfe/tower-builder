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

        public void Setup(AppState appState, TransitionPayload payload)
        {
            base.Setup(appState);
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override void ProcessTick()
        {
            Debug.Log("Idle");
        }

        public override TransitionPayloadBase GetNextState()
        {
            return residentBehavior.GetNextGoalTransitionPayload();
        }
    }
}