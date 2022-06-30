using System.Collections;
using System.Collections.Generic;

using TowerBuilder.State.Residents;
using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public class WorkFurnitureBehavior : RoomFurnitureBehaviorBase
    {
        public WorkFurnitureBehavior(RoomFurnitureBase roomFurniture) : base(roomFurniture) { }

        public override void OnInteractStart(Resident resident) { }
        public override void OnInteractEnd(Resident resident) { }
        public override bool CanInteractWith(Resident resident) { return false; }
    }
}