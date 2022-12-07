using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures.Validators;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class Furniture : Entity
    {
        public override string idKey { get => "furniture"; }

        public int condition { get; private set; } = 100;
        public int price { get; private set; } = 0;

        public FurnitureValidatorBase validator { get; private set; }

        // TODO - are multiples per cell allowed?
        public int homeSlotCount = 0;

        public FurnitureTemplate template;

        public override int pricePerCell => template.price;

        public Furniture(FurnitureTemplate furnitureTemplate)
        {
            this.template = furnitureTemplate;

            this.key = furnitureTemplate.key;
            this.title = furnitureTemplate.title;
            this.category = furnitureTemplate.title;

            this.price = furnitureTemplate.price;
            this.homeSlotCount = furnitureTemplate.homeSlotCount;

            this.validator = furnitureTemplate.furnitureValidatorFactory(this);

            this.cellCoordinatesList = CellCoordinatesList.CreateRectangle(template.dimensions.width, template.dimensions.height);
        }

        public override string ToString()
        {
            return $"Furniture {id}";
        }
    }
}