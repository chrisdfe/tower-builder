using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes.Validators;
using TowerBuilder.DataTypes.Validators.Entities;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                ValidateEntrancesAndExits
            };

        public TransportationItemValidator(TransportationItem transportationItem) : base(transportationItem) { }

        static ListWrapper<ValidationError> ValidateEntrancesAndExits(AppState appState, Entity entity)
        {
            TransportationItem transportationItem = entity as TransportationItem;
            foreach (CellCoordinates cellCoordinates in transportationItem.entranceCellCoordinatesList.items)
            {
                if (!GenericEntityValidations.IsValidStandardLocation(appState, cellCoordinates))
                {
                    return Validator.CreateSingleItemValidationErrorList($"Invalid entrance.");
                }
            }

            foreach (CellCoordinates cellCoordinates in transportationItem.exitCellCoordinatesList.items)
            {
                if (!GenericEntityValidations.IsValidStandardLocation(appState, cellCoordinates))
                {
                    return Validator.CreateSingleItemValidationErrorList($"Invalid exit.");
                }
            }

            return new ListWrapper<ValidationError>();
        }
    }
}