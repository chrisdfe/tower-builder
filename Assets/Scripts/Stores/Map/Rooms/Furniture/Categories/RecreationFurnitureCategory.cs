using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Furniture.Configs;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Categories
{
    public class RecreationFurnitureCategory : RoomFurnitureBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Food; } }

        public int occupancy;
        public float funPerTick;

        public RecreationFurnitureCategory(Room room, FoodFurnitureConfig config) : base(room, config)
        {
            this.funPerTick = config.foodPerTick;
            this.occupancy = config.occupancy;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}