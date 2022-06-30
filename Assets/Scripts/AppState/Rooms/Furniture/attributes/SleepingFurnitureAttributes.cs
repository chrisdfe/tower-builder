using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public class SleepingFurnitureAttributes : RoomFurnitureAttributesBase
    {
        public override RoomFurnitureCategory category { get { return RoomFurnitureCategory.Sleeping; } }

        public float restPerTick;
    }
}