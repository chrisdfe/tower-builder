using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Motors
{
    public class ResidentMotor
    {
        public Resident resident { get; private set; }

        public Route currentRoute { get; private set; }
        public int currentRouteSegmentIndex { get; private set; } = 0;
        public RouteSegment currentRouteSegment { get; private set; }
        public int currentRouteSegmentCellStepIndex { get; private set; } = 0;
        public CellCoordinates currentRouteSegmentCellStep { get; private set; }

        public ResidentMotor(Resident resident)
        {
            this.resident = resident;
        }

        public void Setup() { }

        public void Teardown() { }

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
            return resident.cellCoordinates == currentRoute.destination;
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
            currentRouteSegmentCellStepIndex = index;
            currentRouteSegmentCellStep = currentRouteSegment.cellSteps[index];
            resident.cellCoordinates = currentRouteSegmentCellStep;
        }
    }
}