using System;
using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.DataTypes;
using TowerBuilder.DataTypes.EntityGroups;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities.Foundations
{
    public class FoundationDestroyValidator : EntityValidator
    {
        protected override List<EntityValidator.ValidationFunc> customValidators =>
            new List<EntityValidator.ValidationFunc>()
            {
                ValidateNoFoundationsAreAbove,
                ValidateNothingElseIsInRoom
            };

        public FoundationDestroyValidator(Foundation foundation) : base(foundation) { }

        static List<ValidationError> ValidateNoFoundationsAreAbove(AppState appState, Entity entity)
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

            return new List<ValidationError>();
        }

        static List<ValidationError> ValidateNothingElseIsInRoom(AppState appState, Entity entity)
        {
            EntityGroup parentRoom = appState.EntityGroups.Rooms.FindEntityParent(entity);

            // Filter out current foundation
            List<Entity> entitiesInRoom =
                parentRoom.childEntities.items.FindAll(entity => (
                    // Filter out Foundations
                    entity.GetType() != typeof(Foundation) &&
                    // Ignore entities marked for deletion
                    !entity.isMarkedForDeletion
                ));
            if (entitiesInRoom.Count > 0)
            {
                return EntityValidator.CreateSingleItemValidationErrorList("Cannot remove room with things inside of it.");
            }

            return new List<ValidationError>();
        }
    }
}