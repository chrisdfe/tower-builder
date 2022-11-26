using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class CockpitBehavior : FurnitureBehaviorBase
    {
        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Cockpit }; } }

        public CockpitBehavior(Furniture furniture) : base(furniture) { }
    }
}