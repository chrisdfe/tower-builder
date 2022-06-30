using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public class WorkFurnitureAttributes : RoomFurnitureAttributesBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Work; } }

        public int incomePerTick;
    }
}