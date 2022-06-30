using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public class RecreationFurnitureAttributes : RoomFurnitureAttributesBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Recreation; } }

        public int occupancy;
        public float funPerTick;
    }
}