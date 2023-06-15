using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                ValidateIsOnGroundFloorOrAboveAnotherFoundation
            };

        public FoundationValidator(Foundation foundation) : base(foundation) { }

        static ListWrapper<ValidationError> ValidateIsOnGroundFloorOrAboveAnotherFoundation(AppState appState, Entity entity)
        {
            return new ListWrapper<ValidationError>();
        }
    }
}