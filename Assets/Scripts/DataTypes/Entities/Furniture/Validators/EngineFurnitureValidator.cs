using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Furnitures.Validators
{
    public class EngineFurnitureValidator : FurnitureValidatorBase
    {
        public override List<FurnitureValidationFunc> validations
        {
            get
            {
                return new List<FurnitureValidationFunc>() {
                    GenericFurnitureValidations.CreateValidateFurnitureIsNotOnFloor(0)
                };
            }
        }

        public EngineFurnitureValidator(Furniture furniture) : base(furniture) { }
    }
}