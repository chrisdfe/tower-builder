using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.TransportationItems
{
    public class TransportationItem
    {
        // public class Node
        // {
        //     public Room room;
        //     public CellCoordinates cellCoordinates;

        //     public bool Matches(Node otherNode)
        //     {
        //         return (
        //             otherNode.room == room &&
        //             otherNode.cellCoordinates == cellCoordinates
        //         );
        //     }
        // }

        public int id { get; private set; }

        public string title { get; private set; } = "None";
        public string key { get; private set; } = "None";
        public string category { get; private set; } = "None";

        public int pricePerCell { get; private set; } = 0;

        public CellCoordinatesList cellCoordinatesList;

        public CellCoordinates entranceCellCoordinates = CellCoordinates.zero;
        public CellCoordinates exitCellCoordinates = CellCoordinates.zero;

        public bool isInBlueprintMode = false;

        // public Node entranceNode;
        // public Node exitNode;

        public bool isOneWay = false;

        public TransportationItemTemplate template;

        public TransportationItem(TransportationItemTemplate template)
        {
            this.id = UIDGenerator.Generate("TransportationItem");

            this.title = template.title;
            this.key = template.key;
            this.category = template.category;
            this.pricePerCell = template.pricePerCell;

            this.cellCoordinatesList = template.cellCoordinatesList.Clone();

            this.entranceCellCoordinates = template.entranceCellCoordinates;
            this.exitCellCoordinates = template.exitCellCoordinates;

            this.template = template;
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        /*
            Public Interface
        */
        public void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            cellCoordinatesList.PositionAtCoordinates(cellCoordinates);
            entranceCellCoordinates = CellCoordinates.Add(cellCoordinates, template.entranceCellCoordinates);
            exitCellCoordinates = CellCoordinates.Add(cellCoordinates, template.exitCellCoordinates);
        }

        // public bool ConnectsToRoom(Room room)
        // {
        //     return (
        //         entranceNode.room == room ||
        //         exitNode.room == room
        //     );
        // }

        // public bool ContainsNode(Node node)
        // {
        //     return node != null && (entranceNode.Matches(node) || exitNode.Matches(node));
        // }

        public CellCoordinates GetEntranceOrExit(CellCoordinates cellCoordinates)
        {
            if (cellCoordinates.Matches(entranceCellCoordinates))
            {
                return exitCellCoordinates;
            }

            if (cellCoordinates.Matches(exitCellCoordinates))
            {

            }

            return null;
        }

        // public Node GetOtherNode(Node node)
        // {
        //     if (ContainsNode(node))
        //     {
        //         if (node.Matches(entranceNode))
        //         {
        //             return exitNode;
        //         }

        //         if (node.Matches(exitNode))
        //         {
        //             return entranceNode;
        //         }
        //     }

        //     return null;
        // }

        // public Node FindNodeByRoom(Room room)
        // {
        //     if (entranceNode.room == room)
        //     {
        //         return entranceNode;
        //     }

        //     if (exitNode.room == room)
        //     {
        //         return exitNode;
        //     }

        //     return null;
        // }
    }
}