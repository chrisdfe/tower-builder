using System.Collections.Generic;
using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.Entities.Foundations;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public static class GenericEntityValidations
    {
        public static List<ValidationError> ValidateWalletHasEnoughMoney(AppState appState, Entity entity)
        {
            if (appState.Wallet.balance < entity.price)
            {
                return Validator.CreateSingleItemValidationErrorList("Insufficient funds.");
            }

            return new List<ValidationError>();
        }

        public static List<ValidationError> ValidateIsOnFloor(AppState appState, Entity entity)
        {
            foreach (CellCoordinates cellCoordinates in appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).bottomRow.items)
            {
                Entity foundation = appState.Entities.Foundations.FindEntityAtCell(cellCoordinates);

                // If this entity is not in a foundation that is outside of the scope of this validator
                if (foundation != null)
                {
                    int bottomFloorOfFoundation = appState.EntityGroups.GetAbsoluteCellCoordinatesList(foundation).lowestY;
                    ListWrapper<Entity> floors = appState.Entities.Floors.FindEntitiesAtCell(cellCoordinates.coordinatesBelow);

                    if (cellCoordinates.y != bottomFloorOfFoundation && floors.Count == 0)
                    {
                        return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed on floor.");
                    }
                }
            }

            return new List<ValidationError>();
        }

        public static List<ValidationError> ValidateEntityIsNotOverlappingAnotherEntityOfSameType(AppState appState, Entity entity)
        {
            EntityStateSliceBase stateSlice = appState.Entities.GetStateSlice(entity);



            Entity overlappingEntity = stateSlice.list.Find((otherEntity) =>
                otherEntity != entity &&
                appState.EntityGroups.GetAbsoluteCellCoordinatesList(otherEntity)
                    .OverlapsWith(
                        appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity)
                    )
            );

            if (overlappingEntity != null)
            {
                return Validator.CreateSingleItemValidationErrorList($"You cannot build {entity.typeLabel}s on top of each other.");
            }

            return new List<ValidationError>();
        }

        public static List<ValidationError> ValidateEntityIsNotUnderground(AppState appState, Entity entity)
        {
            foreach (int y in appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).yValues)
            {
                if (y < 0)
                {
                    return Validator.CreateSingleItemValidationErrorList($"You cannot build {entity.typeLabel} underground.");
                }
            }

            return new List<ValidationError>();
        }

        public static List<ValidationError> ValidateEntityIsInsideFoundation(AppState appState, Entity entity)
        {
            string errorMessage = $"{entity.typeLabel} must be built inside of a valid foundation";

            foreach (CellCoordinates cellCoordinates in appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).items)
            {
                ListWrapper<Entity> foundationsAtCell = appState.Entities.Foundations.FindEntitiesAtCell(cellCoordinates);

                if (foundationsAtCell.Count == 0)
                {
                    return Validator.CreateSingleItemValidationErrorList(errorMessage);
                }

                foreach (Entity foundationEntity in foundationsAtCell.items)
                {
                    if (!foundationEntity.isInBlueprintMode && !foundationEntity.buildValidator.isValid)
                    {
                        return Validator.CreateSingleItemValidationErrorList(errorMessage);
                    }
                }
            }

            return Validator.CreateEmptyValidationErrorList();
        }

        /*
            Validation creators
        */
        public static EntityValidator.ValidationFunc CreateValidateEntityIsAtYCoordinate(int y) =>
            (AppState appState, Entity entity) =>
            {
                if (appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).lowestY != y)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed on y {y + 1}");
                }

                return new List<ValidationError>();
            };

        public static EntityValidator.ValidationFunc CreateValidateEntityCellIsNotAtYCoordinate(int y) =>
            (AppState appState, Entity entity) =>
            {
                if (appState.EntityGroups.GetAbsoluteCellCoordinatesList(entity).lowestY == y)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must not be placed on y {y + 1}");
                }

                return new List<ValidationError>();
            };

        /* 
            Validations that return a bool not a list of ValidationErrors
        */
        // TODO - this still doesn't seem like the right place for this
        public static bool IsValidStandardLocation(AppState appState, CellCoordinates cellCoordinates) =>
            IsInsideRoom(appState, cellCoordinates)
                ? IsValidInsideEntityLocation(appState, cellCoordinates)
                : IsValidOutsideEntityLocation(appState, cellCoordinates);

        public static bool IsInsideRoom(AppState appState, CellCoordinates cellCoordinates) =>
            appState.Entities.Foundations.FindEntityAtCell(cellCoordinates) != null;

        // outside + on the ground
        public static bool IsValidOutsideEntityLocation(AppState appState, CellCoordinates cellCoordinates) =>
            cellCoordinates.y == 0 &&
            HasEnoughVerticalSpace(appState, cellCoordinates);

        // inside + on a floor
        public static bool IsValidInsideEntityLocation(AppState appState, CellCoordinates cellCoordinates) =>
            appState.Entities.Foundations.FindEntityAtCell(cellCoordinates) != null &&
            // appState.Entities.Floors.FindEntityTypeAtCell(cellCoordinates) != null &&
            HasEnoughVerticalSpace(appState, cellCoordinates);

        public static bool HasEnoughVerticalSpace(AppState appState, CellCoordinates cellCoordinates) => true;
    }
}