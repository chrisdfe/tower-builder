using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Time;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class IdleStateHandler : StateHandlerBase
    {
        public override StateKey key { get; } = StateKey.Idle;

        public class TransitionPayload : TransitionPayloadBase
        {
            public override StateKey key { get; } = StateKey.Idle;
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
        }

        public override TransitionPayloadBase GetNextState()
        {
            return residentBehavior.GetNextGoalTransitionPayload();
        }
    }
}