using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateEntityIsInsideFoundation,
            };

        public FloorValidator(Floor floor) : base(floor) { }

    }
}