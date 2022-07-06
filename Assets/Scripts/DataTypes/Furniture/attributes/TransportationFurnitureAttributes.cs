using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furniture
{
    public class TransportationFurnitureAttributes : FurnitureAttributesBase
    {
        public override FurnitureCategory category { get { return FurnitureCategory.Transportation; } }

        public int occupancy;
        public int speedPerTick;
    }
}