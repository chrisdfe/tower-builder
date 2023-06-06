using System;
using System.Collections.Generic;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public FreightValidator(FreightItem freight) : base(freight) { }

    }
}