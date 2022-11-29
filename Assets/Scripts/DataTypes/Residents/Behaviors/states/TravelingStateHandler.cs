using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public class TravelingStateHandler : StateHandlerBase
    {
        public class TransitionPayload : TransitionPayloadBase
        {
            public override StateKey key { get; } = StateKey.Traveling;
            public Route route;
        }

        public Route route { get; private set; }
        public RouteProgress routeProgress { get; private set; }

        public TravelingStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior) { }

        public void Setup(AppState appState, TransitionPayload payload)
        {
            base.Setup(appState);

            this.routeProgress = new RouteProgress(payload.route);

            appState.Residents.SetResidentPosition(residentBehavior.resident, routeProgress.currentCell);
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override TransitionPayloadBase GetNextState()
        {
            if (routeProgress.isAtEndOfRoute)
            {
                return residentBehavior.GetNextGoalTransitionPayload();
            }

            return null;
        }

        public override void ProcessTick()
        {
            routeProgress.IncrementProgress();

            appState.Residents.SetResidentPosition(residentBehavior.resident, routeProgress.currentCell);

            if (routeProgress.isAtEndOfRoute)
            {
                residentBehavior.CompleteCurrentGoal();
            }
        }
    }
}