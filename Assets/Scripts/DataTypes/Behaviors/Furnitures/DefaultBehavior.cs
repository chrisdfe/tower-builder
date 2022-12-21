using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public class DefaultBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Default;

        public DefaultBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }
    }
}