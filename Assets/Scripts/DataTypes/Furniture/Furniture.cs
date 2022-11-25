using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Furnitures.Validators;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class Furniture
    {
        public int id { get; private set; }

        public int condition { get; private set; } = 100;

        public string key { get; private set; } = "None";
        public string title { get; private set; } = "None";
        public string category { get; private set; } = "None";
        public int price { get; private set; } = 0;

        public FurnitureValidatorBase validator { get; private set; }

        public bool isInBlueprintMode = false;
        public int homeSlotCount = 0;

        public Room room;
        public CellCoordinates cellCoordinates = CellCoordinates.zero;

        public FurnitureTemplate template;

        public Furniture(FurnitureTemplate furnitureTemplate)
        {
            this.id = UIDGenerator.Generate("furniture");

            this.key = furnitureTemplate.key;
            this.title = furnitureTemplate.title;
            this.category = furnitureTemplate.title;
            this.price = furnitureTemplate.price;
            this.homeSlotCount = furnitureTemplate.homeSlotCount;

            this.template = furnitureTemplate;

            this.validator = furnitureTemplate.furnitureValidatorFactory(this);
        }

        public override string ToString()
        {
            return $"Furniture {id}";
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