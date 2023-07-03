using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationBuildValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                ValidateIsOnGroundFloorOrAboveAnotherFoundation
            };

        public FoundationBuildValidator(Foundation foundation) : base(foundation) { }

        static ListWrapper<ValidationError> ValidateIsOnGroundFloorOrAboveAnotherFoundation(AppState appState, Entity entity)
        {
            CellCoordinatesList absoluteCellCoordinatesList = appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity);
            int bottomFloor = absoluteCellCoordinatesList.lowestY;

            if (bottomFloor > 0)
            {
                foreach (CellCoordinates bottomRowCellCoordinates in absoluteCellCoordinatesList.bottomRow.items)
                {
                    Entity foundationBelow = appState.Entities.Foundations.FindEntityAtCell(
                        bottomRowCellCoordinates.coordinatesBelow
                    );

                    if (foundationBelow == null)
                    {
                        return CreateSingleItemValidationErrorList("Rooms must be built on ground floor or above another room");
                    }
                }
            }

            return new ListWrapper<ValidationError>();
        }
    }
}