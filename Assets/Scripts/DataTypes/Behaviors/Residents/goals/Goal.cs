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
        public abstract class Goal
        {
            // public enum Priority
            // {
            //     Lowest,
            //     Low,
            //     Medium,
            //     High,
            //     Important,
            //     Emergency,
            // }

            public virtual string title { get; } = "Goal";
            public bool isComplete = false;
            public bool hasBegun = false;
            // public Priority priority = Priority.Medium;
        }
    }
}