using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public class TransportationFurnitureAttributes : RoomFurnitureAttributesBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Transportation; } }

        public int occupancy;
        public int speedPerTick;
    }
}