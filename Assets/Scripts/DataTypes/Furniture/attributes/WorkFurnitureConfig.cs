using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class WorkFurnitureAttributes : FurnitureAttributesBase
    {
        public override FurnitureCategory category { get { return FurnitureCategory.Work; } }

        public int incomePerTick;
    }
}