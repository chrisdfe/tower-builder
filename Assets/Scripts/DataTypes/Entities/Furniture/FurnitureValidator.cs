using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public FurnitureValidator(Furniture furniture) : base(furniture) { }

    }
}