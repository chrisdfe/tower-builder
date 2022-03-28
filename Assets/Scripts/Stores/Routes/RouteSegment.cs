using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public class RouteSegment
    {
        // RouteSegmentTransportationType transportationType = RouteSegmentTransportationType.Walking;
        public RouteSegmentNode startNode;
        public RouteSegmentNode endNode;
        public RouteSegmentType type;

        public RouteSegment(RouteSegmentNode startNode, RouteSegmentNode endNode, RouteSegmentType type)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.type = type;
        }

        // public RouteSegment(RoomEntrance start, RoomEntrance end, RouteSegmentTransportationType transportationType)
        // {
        //     this.start = start;
        //     this.end = end;
        //     this.transportationType = transportationType;
        // }

        public RouteSegment Clone()
        {
            // return new RouteSegment(startNode.Clone(), endNode.Clone(), type);
            return new RouteSegment(startNode, endNode, type);
        }
    }
}