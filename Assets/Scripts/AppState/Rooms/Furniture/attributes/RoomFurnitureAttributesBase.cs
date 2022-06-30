using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
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