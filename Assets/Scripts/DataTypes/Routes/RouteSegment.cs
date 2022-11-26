using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class RouteSegment
    {
        public enum Type
        {
            WalkingAcrossRoom,
            UsingRoomConnection,
            UsingStairs,
            UsingElevator,
        }

        public class Node
        {
            public CellCoordinates cellCoordinates { get; private set; }
            public Room room { get; private set; }

            public Node(CellCoordinates cellCoordinates, Room room)
            {
                this.cellCoordinates = cellCoordinates;
                this.room = room;
            }

            public Node Clone()
            {
                return new Node(cellCoordinates.Clone(), room);
            }
        }

        public Node startNode;
        public Node endNode;
        public Type type;

        public List<CellCoordinates> cellSteps { get; private set; } = new List<CellCoordinates>();
        public int distance { get { return cellSteps.Count; } }

        public RouteSegment(Node startNode, Node endNode, Type type)
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