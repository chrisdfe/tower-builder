using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class BedBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Bed;

        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Sleeping }; } }

        public BedBehavior(Furniture furniture) : base(furniture)
        {
        }
    }
}