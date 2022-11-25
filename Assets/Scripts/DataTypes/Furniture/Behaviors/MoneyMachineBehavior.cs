using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Behaviors
{
    public class MoneyMachineBehavior : FurnitureBehaviorBase
    {
        public override FurnitureBehaviorTag[] tags
        {
            get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Industry }; }
        }

        public MoneyMachineBehavior(Furniture furniture) : base(furniture) { }

        public override void InteractTick(AppState appState)
        {
            base.InteractTick(appState);

            appState.Wallet.AddBalance(1000);
        }
    }
}