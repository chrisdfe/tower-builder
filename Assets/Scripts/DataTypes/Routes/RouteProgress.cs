using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class RouteProgress
    {
        public Route route { get; private set; }

        public bool isAtEndOfRoute => currentSegmentIndex >= route.segments.Count - 1;

        int currentSegmentIndex = 0;
        // This will only be true at the very beginning of the route, since one segment's endNode is the next's startNode
        bool isAtFirstCell = true;

        public CurrentAndNext<CellCoordinates> currentAndNextCell
        {
            get
            {
                return new CurrentAndNext<CellCoordinates>(currentCell, nextCell);
            }
        }

        public RouteSegment currentSegment
        {
            get
            {
                return route.segments[currentSegmentIndex];
            }
        }

        public CellCoordinates currentCell
        {
            get
            {
                if (isAtFirstCell) return currentSegment.startNode.cellCoordinates;
                return currentSegment.endNode.cellCoordinates;
            }
        }

        public int nextSegmentIndex
        {
            get
            {
                // avoid out of bounds errors
                if (isAtEndOfRoute) return currentSegmentIndex;
                return currentSegmentIndex + 1;
            }
        }

        public RouteSegment nextSegment => route.segments[nextSegmentIndex];

        public CellCoordinates nextCell
        {
            get
            {
                if (isAtFirstCell) return currentSegment.endNode.cellCoordinates;
                return nextSegment.endNode.cellCoordinates;
            }
        }

        public RouteProgress(Route route)
        {
            this.route = route;
        }

        public void IncrementProgress()
        {
            if (isAtFirstCell)
            {
                isAtFirstCell = false;
            }
            else
            {
                currentSegmentIndex++;
            }
        }
    }
}