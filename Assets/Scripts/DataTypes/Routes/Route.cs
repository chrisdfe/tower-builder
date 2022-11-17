using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class Route
    {
        public CellCoordinates start { get { return this.segments.First().startNode.cellCoordinates; } }
        public CellCoordinates destination { get { return this.segments.Last().endNode.cellCoordinates; } }

        public List<RouteSegment> segments { get; private set; }

        public RouteSegmentNode firstSegmentNode
        {
            get
            {
                return segments[0].startNode;
            }
        }

        public RouteSegmentNode lastSegmentNode
        {
            get
            {
                return segments[segments.Count - 1].endNode;
            }
        }

        public Route(List<RouteSegment> segments)
        {
            this.segments = segments;
        }

        public Route(RouteAttempt routeAttempt)
        {
            this.segments = routeAttempt.routeSegments;
        }
    }
}