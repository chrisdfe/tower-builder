using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;

namespace TowerBuilder.DataTypes.Entities.TransportationItems
{
    public class TransportationItemValidator : EntityValidator
    {
        protected override List<EntityValidationFunc> customValidators =>
            new List<EntityValidationFunc>()
            {
                ValidateEntrancesAndExits
            };

        public TransportationItemValidator(TransportationItem transportationItem) : base(transportationItem) { }

        static EntityValidationErrorList ValidateEntrancesAndExits(AppState appState, Entity entity)
        {
            TransportationItem transportationItem = entity as TransportationItem;
            foreach (CellCoordinates cellCoordinates in transportationItem.entranceCellCoordinatesList.items)
            {
                if (!GenericEntityValidations.IsValidStandardLocation(appState, cellCoordinates))
                {
                    return new EntityValidationErrorList($"Invalid entrance.");
                }
            }

            foreach (CellCoordinates cellCoordinates in transportationItem.exitCellCoordinatesList.items)
            {
                if (!GenericEntityValidations.IsValidStandardLocation(appState, cellCoordinates))
                {
                    return new EntityValidationErrorList($"Invalid exit.");
                }
            }

            return new EntityValidationErrorList();
        }
    }
}