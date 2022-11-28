using System.Collections;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class DefaultBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Default;

        public DefaultBehavior(Furniture furniture) : base(furniture) { }
    }
}