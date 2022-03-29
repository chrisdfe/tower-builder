using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public class RoomFurnitureAttributesBase
    {
        public virtual RoomFurnitureCategory category { get { return RoomFurnitureCategory.None; } }

        public RoomFurnitureOwnability ownability;
        public float roomBeautyScore;
        public int price;

        // Relative coordinates
        public CellCoordinates coordinates;
    }
}