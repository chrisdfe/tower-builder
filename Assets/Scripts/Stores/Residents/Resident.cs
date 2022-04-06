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

        public ResidentMotor motor { get; private set; }
        public ResidentMood mood { get; private set; }
        public ResidentNeeds needs { get; private set; }

        public Route currentRoute { get; private set; }
        public int currentRouteSegmentIndex { get; private set; } = 0;
        public RouteSegment currentRouteSegment { get; private set; }
        public int currentRouteSegmentCellStepIndex { get; private set; } = 0;
        public CellCoordinates currentRouteSegmentCellStep { get; private set; }

        public Resident()
        {
            this.motor = new ResidentMotor(this);
            this.mood = new ResidentMood(this);
            this.needs = new ResidentNeeds(this);
        }

        public void StartOnRoute(Route route)
        {
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
            // Since the 1st cellStep is going to always have the same coordinates as
            // the last step of the previous segment, and that segments will always have
            // at least 2 nodes, we can safely go to the 2nd cellStep instead of the 1st
            GoToSegmentCellStep(1);
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