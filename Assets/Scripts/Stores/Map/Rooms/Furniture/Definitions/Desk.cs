using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Definitions
{
    public class DeskConfig : RoomConfigBase
    {
        public int occupancy;
    }

    public class Desk : RoomFurnitureBase
    {
        public int occupancy { get; private set; }

        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Work; } }

        public Desk(Room room, DeskConfig config) : base(room, config)
        {
            this.occupancy = config.occupancy;
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}