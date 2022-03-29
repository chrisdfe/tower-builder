using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Furniture.Configs;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Categories
{
    public class SleepingFurnitureCategory : RoomFurnitureBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Sleeping; } }

        public float restPerTick;

        public SleepingFurnitureCategory(Room room, SleepingFurnitureConfig config) : base(room, config)
        {
            this.restPerTick = config.restPerTick;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}