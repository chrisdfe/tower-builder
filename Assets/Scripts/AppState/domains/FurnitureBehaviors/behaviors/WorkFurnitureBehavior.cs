using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Residents;
using UnityEngine;

namespace TowerBuilder.State.Rooms.Furnitures
{
    public class WorkFurnitureBehavior : FurnitureBehaviorBase
    {
        public WorkFurnitureBehavior(Furniture furniture) : base(furniture) { }

        public override void OnInteractStart(Resident resident) { }
        public override void OnInteractEnd(Resident resident) { }
        public override bool CanInteractWith(Resident resident) { return false; }
    }
}