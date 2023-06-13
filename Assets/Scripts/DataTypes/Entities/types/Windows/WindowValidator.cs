using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Windows
{
    public class WindowValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                GenericEntityValidations.ValidateEntityIsInsideFoundation,
                GenericEntityValidations.ValidateEntityIsNotUnderground
            };

        public WindowValidator(Window window) : base(window) { }

    }
}