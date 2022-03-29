using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Configs
{
    public class RecreationFurnitureConfig : RoomFurnitureConfigBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Recreation; } }

        public int occupancy;
        public float funPerTick;
    }
}