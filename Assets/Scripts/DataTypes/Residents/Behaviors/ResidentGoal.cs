using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Furnitures;
using TowerBuilder.DataTypes.Routes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Residents.Behaviors
{
    public abstract class GoalBase { }

    public class TravelGoal : GoalBase
    {
        public Route route;
    }

    public class InteractingWithFurnitureGoal : GoalBase
    {
        public Furniture targetFurniture;
    }
}