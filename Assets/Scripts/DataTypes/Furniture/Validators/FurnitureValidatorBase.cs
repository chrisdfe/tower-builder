using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class FurnitureValidationError : ValidationErrorBase
    {
        public FurnitureValidationError(string message) : base(message) { }
    }

    public abstract class FurnitureValidatorBase : ValidatorBase<FurnitureValidationError>
    {
        protected Furniture furniture;

        public FurnitureValidatorBase(Furniture furniture)
        {
            this.furniture = furniture;
        }

        public override void Validate(AppState appState)
        {
            List<FurnitureValidationError> errors = new List<FurnitureValidationError>();

            // For now furniture is not allowed outside
            Room furnitureRoom = appState.Rooms.queries.FindRoomAtCell(furniture.cellCoordinates);
            if (furnitureRoom == null)
            {
                errors.Add(new FurnitureValidationError("furniture must be placed inside."));
            }

            this.errors = errors;
        }
    }

    public class DefaultFurnitureValidator : FurnitureValidatorBase
    {
        public DefaultFurnitureValidator(Furniture furniture) : base(furniture)
        {
            Debug.Log("New default furniture validator");
        }

        public override void Validate(AppState appState)
        {
            base.Validate(appState);
        }
    }
}