using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using UnityEngine;

namespace TowerBuilder.DataTypes.Behaviors.Furnitures
{
    public class MoneyMachineBehavior : FurnitureBehavior
    {
        public override Key key { get; } = FurnitureBehavior.Key.MoneyMachine;

        public MoneyMachineBehavior(AppState appState, Furniture furniture) : base(appState, furniture) { }

        protected override void OnInteractTick(Resident resident)
        {
            appState.Wallet.AddBalance(1000);
        }
    }
}