using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Behaviors.Furnitures;
using UnityEngine;

namespace TowerBuilder.DataTypes.Validators.Behaviors.Furnitures
{
    public class FurnitureBehaviorValidator : Validator<FurnitureBehavior>
    {
        FurnitureBehavior furnitureValidationBehavior => validationItem as FurnitureBehavior;

        public FurnitureBehaviorValidator(FurnitureBehavior FurnitureBehavior) : base(FurnitureBehavior) { }
    }
}