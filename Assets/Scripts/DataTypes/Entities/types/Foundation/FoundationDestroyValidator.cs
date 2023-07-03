using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationDestroyValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                ValidateNoFoundationsAreAbove
            };

        public FoundationDestroyValidator(Foundation foundation) : base(foundation) { }

        static ListWrapper<ValidationError> ValidateNoFoundationsAreAbove(AppState appState, Entity entity)
        {
            CellCoordinatesList topRow = appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).topRow;

            foreach (CellCoordinates cellCoordinates in topRow.items)
            {
                Entity foundationAbove = appState.Entities.Foundations.FindEntityAtCell(
                    CellCoordinates.Add(cellCoordinates, new CellCoordinates(0, 1))
                );

                if (foundationAbove != null)
                {
                    return EntityValidator.CreateSingleItemValidationErrorList("Cannot remove a foundation below another foundation");
                }
            }

            return new ListWrapper<ValidationError>();
        }
    }
}