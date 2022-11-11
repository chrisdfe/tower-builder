using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures
{
    public class FurnitureAttributesBase
    {
        public virtual FurnitureCategory category { get { return FurnitureCategory.None; } }

        public FurnitureOwnability ownability;
        public float roomBeautyScore;
        public int price;

        // Relative coordinates
        public CellCoordinates coordinates;
    }
}