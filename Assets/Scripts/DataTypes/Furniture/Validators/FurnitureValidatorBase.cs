using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public abstract class FurnitureValidatorBase : ValidatorBase<FurnitureValidationError>
    {
        protected Furniture furniture;

        public List<FurnitureValidationFunc> baseValidations { get; } = new List<FurnitureValidationFunc>() {
            GenericFurnitureValidations.ValidateWalletHasEnoughMoney,
            // All furniture must be inside of rooms for now
            GenericFurnitureValidations.ValidateIsInsideRoom
        };

        public virtual List<FurnitureValidationFunc> validations { get; } = new List<FurnitureValidationFunc>();

        public FurnitureValidatorBase(Furniture furniture)
        {
            this.furniture = furniture;
        }

        public override void Validate(AppState appState)
        {
            List<FurnitureValidationError> errors = new List<FurnitureValidationError>();

            foreach (FurnitureValidationFunc validation in baseValidations.Concat(validations).ToList())
            {
                errors = errors.Concat(validation(appState, furniture)).ToList();
            }

            this.errors = errors;
        }
    }
}