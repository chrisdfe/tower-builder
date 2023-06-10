using TowerBuilder.ApplicationState;
using TowerBuilder.ApplicationState.Entities;
using TowerBuilder.DataTypes.Entities;
using TowerBuilder.DataTypes.Entities.Floors;
using TowerBuilder.DataTypes.EntityGroups.Rooms;
using UnityEngine;

namespace TowerBuilder.DataTypes.Entities
{
    public static class GenericEntityValidations
    {
        public static ListWrapper<ValidationError> ValidateWalletHasEnoughMoney(AppState appState, Entity entity)
        {
            if (appState.Wallet.balance < entity.price)
            {
                return Validator.CreateSingleItemValidationErrorList("Insufficient funds.");
            }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateIsOnFloor(AppState appState, Entity entity)
        {
            foreach (CellCoordinates cellCoordinates in entity.cellCoordinatesList.bottomRow.items)
            {
                CellCoordinates cellBelow = CellCoordinates.Add(cellCoordinates, new CellCoordinates(0, -1));
                ListWrapper<Entity> floors = appState.Entities.Floors.FindEntitiesAtCell(cellBelow);

                if (floors.Count == 0)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed on floor.");
                }
            }

            return new ListWrapper<ValidationError>();

        }

        public static ListWrapper<ValidationError> ValidateEntityIsNotOverlappingAnotherEntity(AppState appState, Entity entity)
        {
            EntityStateSlice stateSlice = appState.Entities.GetStateSlice(entity);

            Entity overlappingEntity = stateSlice.entityList.Find((otherEntity) =>
                otherEntity != entity &&
                otherEntity.cellCoordinatesList.OverlapsWith(entity.cellCoordinatesList)
            );

            if (overlappingEntity != null)
            {
                return Validator.CreateSingleItemValidationErrorList($"You cannot build {entity.typeLabel}s on top of each other.");
            }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateEntityIsNotUnderground(AppState appState, Entity entity)
        {
            foreach (int floor in entity.cellCoordinatesList.floorValues)
            {
                if (floor < 0)
                {
                    return Validator.CreateSingleItemValidationErrorList($"You cannot build {entity.typeLabel} underground.");
                }
            }

            return new ListWrapper<ValidationError>();
        }

        public static ListWrapper<ValidationError> ValidateEntityIsInsideFoundation(AppState appState, Entity entity)
        {
            foreach (CellCoordinates cellCoordinates in entity.cellCoordinatesList.items)
            {
                if (appState.Entities.Foundations.FindEntitiesAtCell(cellCoordinates).Count == 0)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be built inside of a room");
                }
            }

            return Validator.CreateEmptyValidationErrorList();
        }

        /*
            Validation creators
        */
        public static EntityValidator.ValidationFunc CreateValidateEntityIsOnFloor(int floor) =>
            (AppState appState, Entity entity) =>
            {
                if (entity.cellCoordinatesList.lowestFloor != floor)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must be placed on floor {floor + 1}");
                }

                return new ListWrapper<ValidationError>();
            };

        public static EntityValidator.ValidationFunc CreateValidateEntityCellIsNotOnFloor(int floor) =>
            (AppState appState, Entity entity) =>
            {
                if (entity.cellCoordinatesList.lowestFloor == floor)
                {
                    return Validator.CreateSingleItemValidationErrorList($"{entity.typeLabel} must not be placed on floor {floor + 1}");
                }

                return new ListWrapper<ValidationError>();
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
            cellCoordinates.floor == 0 &&
            HasEnoughVerticalSpace(appState, cellCoordinates);

        // inside + on a floor
        public static bool IsValidInsideEntityLocation(AppState appState, CellCoordinates cellCoordinates) =>
            appState.Entities.Foundations.FindEntityAtCell(cellCoordinates) != null &&
            // appState.Entities.Floors.FindEntityTypeAtCell(cellCoordinates) != null &&
            HasEnoughVerticalSpace(appState, cellCoordinates);

        public static bool HasEnoughVerticalSpace(AppState appState, CellCoordinates cellCoordinates) => true;
    }
}