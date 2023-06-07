using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Furnitures
{
    public class FurnitureValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateEntityIsInsideFoundation,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public FurnitureValidator(Furniture furniture) : base(furniture) { }

    }
}