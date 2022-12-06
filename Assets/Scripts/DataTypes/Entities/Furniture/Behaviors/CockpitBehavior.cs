using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Behaviors
{
    public class CockpitBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Cockpit;

        public override FurnitureBehaviorTag[] tags { get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Cockpit }; } }

        public CockpitBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }
    }
}