using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class Furniture
    {
        public FurnitureCategory category { get { return FurnitureCategory.None; } }

        public Room room { get; set; }

        public bool isInBlueprintMode = false;

        public int condition { get; private set; } = 100;

        public CellCoordinates cellCoordinates = CellCoordinates.zero;

        List<FurnitureAttributesBase> attributesList;

        public Furniture() { }

        public Furniture(Room room, List<FurnitureAttributesBase> attributesList)
        {
            this.room = room;
            this.attributesList = attributesList;
        }

        public void OnBuild()
        {
            isInBlueprintMode = false;
        }

        public void OnDestroy()
        {

        }

        public void Setup()
        {

        }

        public void Teardown()
        {

        }
    }
}