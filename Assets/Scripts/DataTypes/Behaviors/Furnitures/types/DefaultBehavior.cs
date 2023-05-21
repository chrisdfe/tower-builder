using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public class DefaultBehavior : FurnitureBehavior
    {
        public override Key key { get; } = FurnitureBehavior.Key.Default;

        public DefaultBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }
    }
}