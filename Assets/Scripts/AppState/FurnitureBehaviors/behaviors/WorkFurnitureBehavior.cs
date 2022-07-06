using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furniture;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.State.Rooms.Furniture
{
    public class WorkFurnitureBehavior : FurnitureBehaviorBase
    {
        public WorkFurnitureBehavior(FurnitureBase roomFurniture) : base(roomFurniture) { }

        public override void OnInteractStart(Resident resident) { }
        public override void OnInteractEnd(Resident resident) { }
        public override bool CanInteractWith(Resident resident) { return false; }
    }
}