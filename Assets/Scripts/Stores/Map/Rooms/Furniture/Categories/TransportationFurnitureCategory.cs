using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Map.Rooms.Furniture.Configs;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Categories
{
    public class TransportationFurnitureCategory : RoomFurnitureBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Transportation; } }

        public int occupancy;
        public float speedPerTick;

        public TransportationFurnitureCategory(Room room, TransportationFurnitureConfig config) : base(room, config)
        {
            this.speedPerTick = config.speedPerTick;
            this.occupancy = config.occupancy;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}