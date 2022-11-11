using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class SleepingFurnitureAttributes : FurnitureAttributesBase
    {
        public override FurnitureCategory category { get { return FurnitureCategory.Sleeping; } }

        public float restPerTick;
    }
}