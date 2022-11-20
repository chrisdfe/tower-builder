using System.Collections;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using UnityEngine;

namespace TowerBuilder.DataTypes.Furnitures.Validators
{
    public class BedFurnitureValidator : FurnitureValidatorBase
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

        public BedFurnitureValidator(Furniture furniture) : base(furniture) { }
    }
}