using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateEntityIsInsideFoundation,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public FreightValidator(FreightItem freight) : base(freight) { }

    }
}