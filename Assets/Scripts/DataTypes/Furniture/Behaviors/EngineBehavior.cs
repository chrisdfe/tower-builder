using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class EngineBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.Engine;

        public override FurnitureBehaviorTag[] tags
        {
            get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Engine }; }
        }

        public EngineBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }
    }
}