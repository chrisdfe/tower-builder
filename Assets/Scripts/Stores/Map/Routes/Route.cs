using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Routes
{
    public class Route
    {
        RouteNode start;
        RouteNode destination;

        public List<RouteSegment> segments { get; private set; }

        public Route() { }

        public void Build() { }
    }
}