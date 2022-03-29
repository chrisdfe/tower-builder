using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Furniture.Configs;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Categories
{
    public class FoodFurnitureCategory : RoomFurnitureBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Food; } }

        public float foodPerTick;

        public FoodFurnitureCategory(Room room, FoodFurnitureConfig config) : base(room, config)
        {
            this.foodPerTick = config.foodPerTick;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}