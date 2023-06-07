using System;
using System.Collections.Generic;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
            };

        public FoundationValidator(Foundation foundation) : base(foundation) { }

    }
}