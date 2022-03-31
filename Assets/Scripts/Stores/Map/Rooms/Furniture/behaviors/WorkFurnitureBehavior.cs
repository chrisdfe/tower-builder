using System.Collections;
using System.Collections.Generic;
using TowerBuilder.Stores.Map;
using TowerBuilder.Stores.Residents;
using UnityEngine;

namespace TowerBuilder.Stores.Map.Rooms.Furniture
{
    public class WorkFurnitureBehavior : RoomFurnitureBehaviorBase
    {
        public WorkFurnitureBehavior(RoomFurnitureBase roomFurniture) : base(roomFurniture) { }

        public override void OnInteractStart(Resident resident) { }
        public override void OnInteractEnd(Resident resident) { }
    }
}