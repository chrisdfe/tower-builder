using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Furniture.Configs;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Categories
{
    public class WorkFurnitureCategory : RoomFurnitureBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Work; } }

        public int incomePerTick;

        public WorkFurnitureCategory(Room room, WorkFurnitureConfig config) : base(room, config)
        {
            this.incomePerTick = config.incomePerTick;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}