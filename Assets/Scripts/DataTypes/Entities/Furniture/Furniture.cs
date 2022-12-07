using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Furnitures.Validators;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class Furniture : Entity<Furniture.Key>
    {
        public enum Key
        {
            None,
            Bed,
            Engine,
            PilotSeat,
            MoneyMachine
        }

        public static EnumStringMap<Key> KeyLabelMap = new EnumStringMap<Key>(
            new Dictionary<Key, string>() {
                { Key.Bed,          "Bed" },
                { Key.Engine,       "Engine" },
                { Key.PilotSeat,    "PilotSeat" },
                { Key.MoneyMachine, "MoneyMachine" }
            }
        );

        public override string idKey { get => "furniture"; }

        public FurnitureValidatorBase validator { get; private set; }

        // TODO - are multiples per cell allowed?
        public int homeSlotCount = 0;

        public new FurnitureTemplate template { get; }

        public override int pricePerCell => template.price;

        public Furniture(FurnitureTemplate furnitureTemplate) : base(furnitureTemplate)
        {
            this.template = furnitureTemplate;

            this.key = furnitureTemplate.key;

            this.homeSlotCount = furnitureTemplate.homeSlotCount;

            this.validator = furnitureTemplate.furnitureValidatorFactory(this);
        }

        public override string ToString()
        {
            return $"Furniture {id}";
        }
    }
}