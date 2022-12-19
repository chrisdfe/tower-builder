using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Freights
{
    public class FreightValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public FreightValidator(FreightItem freight) : base(freight) { }

    }
}