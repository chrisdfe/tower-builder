using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.Stores;

using TowerBuilder.Stores.Rooms;
using TowerBuilder.Stores.Rooms.Entrances;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public class Route
    {
        public CellCoordinates start { get { return this.segments.First().startNode.cellCoordinates; } }
        public CellCoordinates destination { get { return this.segments.Last().endNode.cellCoordinates; } }

        public List<RouteSegment> segments { get; private set; }

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