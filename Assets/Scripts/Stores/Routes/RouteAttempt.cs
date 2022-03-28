using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.Stores;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms;
using UnityEngine;

namespace TowerBuilder.Stores.Routes
{
    public class RouteAttempt
    {
        public RouteStatus status = RouteStatus.Incomplete;

        public List<Room> visitedRooms { get; private set; }
        public List<RouteSegment> routeSegments { get; private set; }

        public RouteAttempt(List<Room> vistedRooms, List<RouteSegment> routeSegments)
        {
            this.visitedRooms = new List<Room>(vistedRooms);
            this.routeSegments = new List<RouteSegment>(routeSegments);
        }

        public void AddVisitedRoom(Room room)
        {
            visitedRooms.Add(room);
        }

        public void AddRouteSegment(RouteSegment routeSegment)
        {
            routeSegments.Add(routeSegment);
        }

        public RouteSegmentNode GetLatestSegmentNode()
        {
            if (routeSegments.Count == 0)
            {
                return null;
            }

            RouteSegment latestSegment = routeSegments.Last();
            return latestSegment.endNode;
        }

        public RouteAttempt Clone()
        {
            return new RouteAttempt(
                new List<Room>(visitedRooms),
                new List<RouteSegment>(routeSegments)
            );
        }
    }
}