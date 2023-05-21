using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class RouteAttempt
    {
        public enum Status
        {
            Incomplete,
            Complete
        }

        public Status status = Status.Incomplete;

        public List<Room> visitedRooms { get; private set; } = new List<Room>();
        public List<RouteSegment> routeSegments { get; private set; } = new List<RouteSegment>();

        public Room currentRoom;
        public CellCoordinates currentCellCoordinates;

        public int distance
        {
            get
            {
                return routeSegments
                    .Select(routeSegment => routeSegment.distance)
                    .Aggregate(0, (acc, segmentDistance) => acc + segmentDistance);
            }
        }

        public RouteSegment.Node latestSegmentNode
        {
            get
            {
                if (routeSegments.Count == 0)
                {
                    return new RouteSegment.Node(currentCellCoordinates, currentRoom);
                }

                RouteSegment latestSegment = routeSegments.Last();
                return latestSegment.endNode;
            }
        }

        public RouteAttempt()
        {
            visitedRooms = new List<Room>();
            routeSegments = new List<RouteSegment>();
        }

        public RouteAttempt(List<Room> vistedRooms, List<RouteSegment> routeSegments)
        {
            this.visitedRooms = new List<Room>(vistedRooms);
            this.routeSegments = new List<RouteSegment>(routeSegments);
        }

        public void AddVisitedRoom(Room room)
        {
            visitedRooms.Add(room);
        }

        public void GoTo(RouteSegment.Node node, RouteSegment.Type travelingType)
        {
            routeSegments.Add(new RouteSegment(latestSegmentNode, node, travelingType));
            currentCellCoordinates = node.cellCoordinates;
            visitedRooms.Add(node.room);
            currentRoom = node.room;
        }

        public RouteAttempt Clone()
        {
            return new RouteAttempt(
                new List<Room>(visitedRooms),
                new List<RouteSegment>(routeSegments)
            )
            {
                currentRoom = currentRoom,
                currentCellCoordinates = currentCellCoordinates
            };
        }
    }
}