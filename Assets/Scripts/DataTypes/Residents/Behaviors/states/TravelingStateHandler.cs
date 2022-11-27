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

        int currentSegmentIndex = 0;
        bool isAtFinalSegmentIndex { get { return currentSegmentIndex >= route.segments.Count - 1; } }
        RouteSegment currentSegment { get { return route.segments[currentSegmentIndex]; } }

        int currentCellStepIndex = 0;
        bool isAtFinalCellStepIndex { get { return currentCellStepIndex >= currentSegment.cellSteps.Count - 1; } }
        CellCoordinates currentCellStep { get { return currentSegment.cellSteps[currentCellStepIndex]; } }

        bool isAtEndOfRoute
        {
            get
            {
                return (
                    isAtFinalSegmentIndex && isAtFinalCellStepIndex
                );
            }
        }

        public TravelingStateHandler(ResidentBehavior residentBehavior) : base(residentBehavior) { }

        public void Setup(AppState appState, TransitionPayload payload)
        {
            base.Setup(appState);

            this.route = payload.route;
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override TransitionPayloadBase GetNextState()
        {
            if (isAtEndOfRoute)
            {
                return residentBehavior.GetNextGoalTransitionPayload();
            }

            return null;
        }

        public override void ProcessTick()
        {
            if (isAtFinalCellStepIndex)
            {
                currentSegmentIndex++;
                currentCellStepIndex = 0;
            }
            else
            {
                currentCellStepIndex++;
            }

            appState.Residents.SetResidentPosition(residentBehavior.resident, currentCellStep);

            if (isAtEndOfRoute)
            {
                residentBehavior.CompleteCurrentGoal();
            }
        }
    }
}