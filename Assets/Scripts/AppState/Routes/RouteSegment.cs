using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.State;

using TowerBuilder.State.Rooms;
using UnityEngine;

namespace TowerBuilder.State.Routes
{
    public class RouteSegment
    {
        // RouteSegmentTransportationType transportationType = RouteSegmentTransportationType.Walking;
        public RouteSegmentNode startNode;
        public RouteSegmentNode endNode;
        public RouteSegmentType type;

        public List<CellCoordinates> cellSteps = new List<CellCoordinates>();
        public int distance { get { return cellSteps.Count; } }

        public RouteSegment(RouteSegmentNode startNode, RouteSegmentNode endNode, RouteSegmentType type)
        {
            this.startNode = startNode;
            this.endNode = endNode;
            this.type = type;

            CalculateCellSteps();
        }

        public RouteSegment Clone()
        {
            return new RouteSegment(startNode, endNode, type);
        }

        void CalculateCellSteps()
        {
            List<CellCoordinates> result = new List<CellCoordinates>();

            // First up/down
            if (endNode.cellCoordinates.floor > startNode.cellCoordinates.floor)
            {
                for (int floor = startNode.cellCoordinates.floor; floor <= endNode.cellCoordinates.floor; floor++)
                {
                    result.Add(new CellCoordinates(startNode.cellCoordinates.x, floor));
                }
            }
            else
            {
                for (int floor = startNode.cellCoordinates.floor; floor >= endNode.cellCoordinates.floor; floor--)
                {
                    result.Add(new CellCoordinates(startNode.cellCoordinates.x, floor));
                }
            }


            // then over
            if (endNode.cellCoordinates.x > startNode.cellCoordinates.x)
            {
                for (int x = startNode.cellCoordinates.x; x <= endNode.cellCoordinates.x; x++)
                {
                    result.Add(new CellCoordinates(x, endNode.cellCoordinates.floor));
                }
            }
            else
            {
                for (int x = startNode.cellCoordinates.x; x >= endNode.cellCoordinates.x; x--)
                {
                    result.Add(new CellCoordinates(x, endNode.cellCoordinates.floor));
                }
            }

            cellSteps = result;
        }
    }
}