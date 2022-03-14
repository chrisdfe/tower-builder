using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Routes
{
    public class RouteSegment
    {
        RouteSegmentTransportationType transportationType;
        public RouteNode start;
        public RouteNode end;

        public RouteSegment(RouteNode start, RouteNode end, RouteSegmentTransportationType transportationType)
        {
            this.start = start;
            this.end = end;
            this.transportationType = transportationType;
        }
    }
}