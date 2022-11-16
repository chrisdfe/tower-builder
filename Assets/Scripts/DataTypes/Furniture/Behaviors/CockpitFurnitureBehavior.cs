using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class CockpitFurnitureBehavior : FurnitureBehaviorBase
    {
        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Cockpit }; } }

        public CockpitFurnitureBehavior(Furniture furniture) : base(furniture)
        {
        }
    }
}