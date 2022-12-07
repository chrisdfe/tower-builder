using System.Collections;
using System.Collections.Generic;
using TowerBuilder;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Rooms;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Routes
{
    public class RouteSegment
    {
        public enum Type
        {
            WalkingAcrossRoom,
            UsingTransportationItem,
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
        }

        public RouteSegment Clone()
        {
            return new RouteSegment(startNode, endNode, type);
        }
    }
}