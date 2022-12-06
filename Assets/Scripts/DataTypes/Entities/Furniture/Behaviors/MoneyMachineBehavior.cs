using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Residents;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Behaviors
{
    public class MoneyMachineBehavior : FurnitureBehaviorBase
    {
        public override Key key { get; } = FurnitureBehaviorBase.Key.MoneyMachine;

        public override FurnitureBehaviorTag[] tags
        {
            get { return new FurnitureBehaviorTag[] { FurnitureBehaviorTag.Industry }; }
        }

        public MoneyMachineBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        public override void InteractTick(Resident resident)
        {
            base.InteractTick(resident);

            appState.Wallet.AddBalance(1000);
        }
    }
}