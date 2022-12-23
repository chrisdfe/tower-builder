using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
            };

        public FloorValidator(Floor floor) : base(floor) { }

    }
}