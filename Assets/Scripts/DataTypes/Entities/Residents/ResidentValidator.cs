using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public ResidentValidator(Resident resident) : base(resident) { }

    }
}