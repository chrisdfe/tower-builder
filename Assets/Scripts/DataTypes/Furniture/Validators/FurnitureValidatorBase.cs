using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class FurnitureValidationError : ValidationErrorBase
    {
        public FurnitureValidationError(string message) : base(message) { }
    }

    public abstract class FurnitureValidatorBase : ValidatorBase<FurnitureValidatorBase>
    {
        protected Furniture furniture;

        public FurnitureValidatorBase(Furniture furniture)
        {
            this.furniture = furniture;
        }
    }

    public class DefaultFurnitureValidator : FurnitureValidatorBase
    {
        public DefaultFurnitureValidator(Furniture furniture) : base(furniture) { }
        public override void Validate(AppState appState) { }
    }
}