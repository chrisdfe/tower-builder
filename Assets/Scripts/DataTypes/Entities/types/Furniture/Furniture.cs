using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class Furniture : Entity
    {
        public override string idKey { get => "furniture"; }

        public Furniture(FurnitureDefinition furnitureDefinition) : base(furnitureDefinition) { }
    }
}