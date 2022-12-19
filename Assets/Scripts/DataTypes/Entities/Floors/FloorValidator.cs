using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Floors
{
    public class FloorValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
            };

        public FloorValidator(Floor floor) : base(floor) { }

    }
}