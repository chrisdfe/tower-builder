using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public class RoomConfigBase
    {
        public RoomFurnitureOwnability ownability;
        public RoomFurnitureCategory category;
        public float roomBeautyScore;
        public int price;

        // Relative coordinates
        public CellCoordinates coordinates;
    }
}