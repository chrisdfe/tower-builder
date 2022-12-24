using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Attributes.Residents;
using TowerBuilder.DataTypes.Entities.Furnitures;
using TowerBuilder.DataTypes.Entities.Residents;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Behaviors.Residents
{
    public partial class ResidentBehavior
    {
        public class TravelGoal : Goal
        {
            public override string title { get => "Walk"; }
            public Route route;
        }
    }
}