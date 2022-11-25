using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.TransportationItems
{
    public class TransportationItem
    {
        public class Node
        {
            CellCoordinates cellCoordinates;
            Room room;
        }

        public int id { get; private set; }

        public string title { get; private set; } = "None";
        public string key { get; private set; } = "None";
        public string category { get; private set; } = "None";

        public int pricePerCell { get; private set; } = 0;

        public CellCoordinatesList cellCoordinatesList;

        public bool isInBlueprintMode = false;

        public Node nodeA;
        public Node nodeb;

        public TransportationItemTemplate template;

        public TransportationItem(TransportationItemTemplate template)
        {
            this.id = UIDGenerator.Generate("TransportationItem");

            this.title = template.title;
            this.key = template.key;
            this.category = template.category;
            this.pricePerCell = template.pricePerCell;

            this.cellCoordinatesList = template.cellCoordinatesList.Clone();
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void PositionAtCoordinates(CellCoordinates cellCoordinates)
        {
            cellCoordinatesList.PositionAtCoordinates(cellCoordinates);
        }
    }
}