using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
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

        public Furniture(FurnitureDefinition furnitureDefinition) : base(furnitureDefinition) { }

        public override string ToString()
        {
            return $"Furniture {id}";
        }
    }
}