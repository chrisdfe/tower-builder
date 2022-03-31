using System.Collections;
using System.Collections.Generic;
using System.Linq;

using TowerBuilder.Stores.Routes;

using UnityEngine;

namespace TowerBuilder.Stores.Residents
{
    public class Resident
    {
        public CellCoordinates coordinates;
        public ResidentBehaviorState behaviorState;

        public Route currentRoute { get; private set; }
        public int currentRouteSegmentIndex { get; private set; } = 0;
        public RouteSegment currentRouteSegment { get; private set; }
        public int currentRouteSegmentCellStepIndex { get; private set; } = 0;
        public CellCoordinates currentRouteSegmentCellStep { get; private set; }

        public void StartOnRoute(Route route)
        {
            // behaviorState = ResidentBehaviorState.Traveling;
            currentRoute = route;
            currentRouteSegmentIndex = 0;
            currentRouteSegment = currentRoute.segments[currentRouteSegmentIndex];
        }

        public void ProgressAlongCurrentRoute()
        {
            if (IsAtFinalSegmentCellStep())
            {
                if (IsAtFinalSegment())
                {
                    // Route is complete
                    Debug.Log("route is complete");
                }
                else
                {
                    ProgressToNextSegment();
                }
            }
            else
            {
                ProgressToNextSegmentCellStep();
            }

            Debug.Log($"currentRouteSegmentIndex: {currentRouteSegmentIndex}");
            Debug.Log($"currentRouteSegmentType: {currentRouteSegment.type}");
            Debug.Log($"currentRouteSegmentCellStepIndex: {currentRouteSegmentCellStepIndex}");
        }

        public bool IsAtFinalSegment()
        {
            return currentRouteSegmentIndex == currentRoute.segments.Count - 1;
        }

        public bool IsAtFinalSegmentCellStep()
        {
            return currentRouteSegmentCellStepIndex == currentRouteSegment.cellSteps.Count - 1;
        }

        public bool IsAtEndOfCurrentRoute()
        {
            return coordinates == currentRoute.destination;
        }

        void ProgressToNextSegment()
        {
            if (currentRouteSegmentIndex < currentRoute.segments.Count)
            {
                GoToSegment(currentRouteSegmentIndex + 1);
            }
        }

        void GoToSegment(int index)
        {
            currentRouteSegmentIndex = index;
            currentRouteSegment = currentRoute.segments[currentRouteSegmentIndex];

            Debug.Log($"going to segment {index}");
            GoToSegmentCellStep(0);
        }


        void ProgressToNextSegmentCellStep()
        {
            if (currentRouteSegmentCellStepIndex < currentRouteSegment.cellSteps.Count)
            {
                GoToSegmentCellStep(currentRouteSegmentCellStepIndex + 1);
            }
        }

        void GoToSegmentCellStep(int index)
        {
            Debug.Log($"going to segment cell step {index}");
            currentRouteSegmentCellStepIndex = index;
            currentRouteSegmentCellStep = currentRouteSegment.cellSteps[index];
            coordinates = currentRouteSegmentCellStep;
        }
    }
}