using System.Collections;
using System.Collections.Generic;

using TowerBuilder.Stores.Map;

using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public class TransportationFurnitureAttributes : RoomFurnitureAttributesBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Transportation; } }

        public int occupancy;
        public int speedPerTick;
    }
}