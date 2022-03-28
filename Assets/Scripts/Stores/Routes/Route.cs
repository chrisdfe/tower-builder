using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public class Route
    {
        RoomEntrance start;
        RoomEntrance destination;

        public List<RouteSegment> segments { get; private set; }

        public Route() { }

        public void Build() { }
    }
}