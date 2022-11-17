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
            public Route route;
            // For now assume that residents always travel between pieces of furniture
            // public Furniture furniture;
        }

        public enum StateType
        {
            Walking,
            UsingStairs
        }

        public Route route { get; private set; }
        public StateType currentState { get; private set; } = StateType.Walking;

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

        public void Setup(TransitionPayload payload)
        {
            this.route = payload.route;
        }

        public override void Teardown()
        {
            base.Teardown();
        }

        public override TransitionPayloadBase GetNextState(AppState appState)
        {
            if (isAtEndOfRoute)
            {
                return residentBehavior.GetNextGoalTransitionPayload();
            }

            return null;
        }

        public override void ProcessTick(AppState appState)
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