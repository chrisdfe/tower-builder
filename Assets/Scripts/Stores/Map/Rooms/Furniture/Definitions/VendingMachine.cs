using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture.Definitions
{
    public class VendingMachineConfig : RoomConfigBase
    {
    }

    public class VendingMachine : RoomFurnitureBase
    {
        public int occupancy { get; private set; }

        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Food; } }

        public VendingMachine(Room room, VendingMachineConfig config) : base(room, config)
        {
        }

        public override void Initialize() { }
        public override void OnDestroy() { }
    }
}