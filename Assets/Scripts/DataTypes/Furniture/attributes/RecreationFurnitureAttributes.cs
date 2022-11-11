using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class RecreationFurnitureAttributes : FurnitureAttributesBase
    {
        public override FurnitureCategory category { get { return FurnitureCategory.Recreation; } }

        public int occupancy;
        public float funPerTick;
    }
}