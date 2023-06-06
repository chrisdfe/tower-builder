using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Residents
{
    public class ResidentValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateIsInsideRoom,
                GenericEntityValidations.ValidateIsOnFloor
            };

        public ResidentValidator(Resident resident) : base(resident) { }

    }
}