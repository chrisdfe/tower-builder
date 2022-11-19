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

        public Room room { get; private set; }

        public int condition { get; private set; } = 100;

        public string key { get; private set; } = "None";
        public string title { get; private set; } = "None";
        public string category { get; private set; } = "None";

        public FurnitureValidatorBase validator { get; private set; }

        public bool isInBlueprintMode = false;
        public CellCoordinates cellCoordinates = CellCoordinates.zero;

        public Furniture()
        {
            this.id = UIDGenerator.Generate("furniture");
        }

        public Furniture(FurnitureTemplate furnitureTemplate, Room room) : this()
        {
            this.room = room;

            this.key = furnitureTemplate.key;
            this.title = furnitureTemplate.title;
            this.category = furnitureTemplate.title;
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