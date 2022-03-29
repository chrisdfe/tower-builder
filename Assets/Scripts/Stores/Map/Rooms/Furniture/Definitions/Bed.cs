using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Definitions
{
    public class BedConfig : RoomConfigBase
    {
        public int occupancy;
    }

    public class Bed : RoomFurnitureBase
    {
        public int occupancy { get; private set; }

        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Sleeping; } }

        public Bed(Room room, BedConfig config) : base(room, config)
        {
            this.occupancy = config.occupancy;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}