using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.EntityGroups;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class Route
    {
        public CellCoordinates start { get { return this.segments.First().startNode.cellCoordinates; } }
        public CellCoordinates destination { get { return this.segments.Last().endNode.cellCoordinates; } }

        public List<RouteSegment> segments { get; private set; }
        List<RouteSegment> rawSegments;

        public Route(List<RouteSegment> segments)
        {
            rawSegments = segments;
        }

        public Route(RouteAttempt routeAttempt)
        {
            rawSegments = routeAttempt.routeSegments;
        }

        // Normalize segment sizes by breaking the multi-cell-width route attempt segments down into 1-cell-width segments
        public void CalculateCellSegments(AppState appState)
        {
            List<RouteSegment> result = new List<RouteSegment>();
            CellCoordinates currentCellCoordinates;

            foreach (RouteSegment segment in rawSegments)
            {
                currentCellCoordinates = segment.startNode.cellCoordinates;
                while (
                    (currentCellCoordinates.x != segment.endNode.cellCoordinates.x) ||
                    (currentCellCoordinates.y != segment.endNode.cellCoordinates.y)
                )
                {
                    CellCoordinates nextCellCoordinates = currentCellCoordinates.Clone();

                    if (currentCellCoordinates.x > segment.endNode.cellCoordinates.x)
                    {
                        nextCellCoordinates.x--;
                    }
                    else if (currentCellCoordinates.x < segment.endNode.cellCoordinates.x)
                    {
                        nextCellCoordinates.x++;
                    }

                    if (currentCellCoordinates.y > segment.endNode.cellCoordinates.y)
                    {
                        nextCellCoordinates.y--;
                    }
                    else if (currentCellCoordinates.y < segment.endNode.cellCoordinates.y)
                    {
                        nextCellCoordinates.y++;
                    }

                    CreateRouteSegment(nextCellCoordinates, segment.type);
                }
            }

            segments = result;

            RouteSegment.Node CreateSegmentNode(CellCoordinates cellCoordinates)
            {
                EntityGroup roomEntityGroup = appState.EntityGroups.Rooms.FindEntityGroupAtCell(cellCoordinates);

                if (roomEntityGroup == null)
                {
                    throw new RouteException("room not found at coordinates: " + cellCoordinates);
                }

                Room room = roomEntityGroup as Room;
                return new RouteSegment.Node(cellCoordinates, room);
            }


            void CreateRouteSegment(CellCoordinates nextCellCoordinates, RouteSegment.Type type)
            {
                result.Add(
                    new RouteSegment(
                        CreateSegmentNode(currentCellCoordinates),
                        CreateSegmentNode(nextCellCoordinates),
                        type
                    )
                );

                currentCellCoordinates = nextCellCoordinates;
            }
        }
    }
}