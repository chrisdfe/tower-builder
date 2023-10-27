using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class InteractingWithFurnitureGoal : ResidentBehavior.Goal
    {
        public override string title { get => $"Use {furniture}"; }
        public Furniture furniture;

        public override void OnTick(AppState appState)
        {
            // throw new System.NotImplementedException();
        }
    }
}