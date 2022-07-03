using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.DataTypes.Rooms.Furniture
{
    public class RecreationFurnitureAttributes : RoomFurnitureAttributesBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Recreation; } }

        public int occupancy;
        public float funPerTick;
    }
}